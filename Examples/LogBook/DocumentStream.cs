using System;
using System.Collections.Generic;
using System.IO;
using LogBook.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.UserUtils;

namespace LogBook
{
    public class DocumentStream : IStream
    {
        #region Fields

        private bool _disposed;
        public PDFSettings Options { get; private set; }
        public DocumentBuilder Builder { get; private set; }

        public string exoRegularFile { get; set; }
        public string exoItalicFile { get;  set; }
        public string exoBoldFile { get;  set; }

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
            var exofontsDir = Path.Combine(projectDir, "Content", "fonts", "Exo");
            exoRegularFile = Path.Combine(exofontsDir, "Exo-Regular.ttf");
            exoItalicFile = Path.Combine(exofontsDir, "Exo-Italic.ttf");
            exoBoldFile = Path.Combine(exofontsDir, "Exo-Bold.ttf");
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

        #endregion Methods
    }
}
