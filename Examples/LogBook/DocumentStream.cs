using System;
using System.Collections.Generic;
using System.IO;
using Gehtsoft.PDFFlow.LogBook.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Core.Events;
using Gehtsoft.PDFFlow.Events.Data;
using Gehtsoft.PDFFlow.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Gehtsoft.PDFFlow.Infrastructure;

namespace Gehtsoft.PDFFlow.LogBook
{
    public class DocumentStream : IStream, IEventHandler<NewPageCreated>, IEventHandler<DocumentClosed>
    {
        #region Fields

        private bool _disposed;
        public PDFSettings Options { get; private set; }
        public DocumentBuilder Builder { get; private set; }
        private IEventHandlerContainer _eventHandlerContainer;
        private IDocumentService _documentService;
        private INativeDocument _nativeDocument;
        private int _startingPage;

        #endregion Fields

        #region Properties

        public string FilePath { get; set; }
        public Stream Stream { get; }

        public int StartingPage
        {
            get => _startingPage;
            set => _startingPage = value;
        }

        public IStreamCoordinator Coordinator { get; set; }

        #endregion Properties

        #region Constructors

        public DocumentStream(string filePath, PDFSettings options)
        {
            FilePath = filePath;
            Options = options;
            Builder = DocumentBuilder.New();
            Init();
        }

        public DocumentStream(Stream stream, PDFSettings options)
        {
            Stream = stream;
            Options = options;
            Builder = DocumentBuilder.New();
            Init();
        }

        private void Init()
        {
            _eventHandlerContainer = Builder.Scope.ServiceProvider.GetService<IEventHandlerContainer>();
            _documentService = Builder.Scope.ServiceProvider.GetService<IDocumentService>();
            _eventHandlerContainer.RegisterHandler<NewPageCreated>(GetHashCode().ToString(), this);
            _eventHandlerContainer.RegisterHandler<DocumentClosed>(GetHashCode().ToString(), this);
            _nativeDocument = Builder.Scope.ServiceProvider.GetService<INativeDocument>();
            LoadFonts();
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Flush();
            }

            _disposed = true;
        }

        private void LoadFonts()
        {
            var projectDir = Directory.GetCurrentDirectory();
            var exofontsDir = Path.Combine(projectDir, "Content", "Fonts\\exo");
            var exoRegular = Path.Combine(exofontsDir, "Exo-Regular.ttf");
            var exoRegularItalic = Path.Combine(exofontsDir, "Exo-Italic.ttf");
            var exoRegularBold = Path.Combine(exofontsDir, "Exo-Bold.ttf");
            var exoRegularBoldItalic = Path.Combine(exofontsDir, "Exo-BoldItalic.ttf");

            _nativeDocument.LoadFontFromTtfFile(exoRegular, true);
            _nativeDocument.LoadFontFromTtfFile(exoRegularItalic, true);
            _nativeDocument.LoadFontFromTtfFile(exoRegularBold, true);
            _nativeDocument.LoadFontFromTtfFile(exoRegularBoldItalic, true);
        }

        public void Write<T>(T data) where T : IEntity => Coordinator?.Input(data);

        public void Write<T>(IEnumerable<T> data) where T : IEntity => Coordinator?.Input(data);

        public void Write(IDictionary<string, object> data) => Coordinator?.Input(data);

        public void Flush()
        {
            if (Stream != null)
                Builder.Build(Stream);
            else
                Builder.Build(FilePath);
        }

        public void Handle(NewPageCreated eventData) => Options?.PageChanged(eventData);

        public void Handle(DocumentClosed eventData)
        {
            _eventHandlerContainer.UnregisterHandler<NewPageCreated>(GetHashCode().ToString());
            _eventHandlerContainer.UnregisterHandler<DocumentClosed>(GetHashCode().ToString());

        }

        #endregion Methods
    }
}
