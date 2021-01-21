using System;
using System.Collections.Generic;
using System.IO;
using LogBook.Model;
using Gehtsoft.PDFFlow.Builder;

namespace LogBook
{
    internal interface IBuilderAccessor
    {
        DocumentBuilder Builder { get; }
    }

    public interface IStream:IDisposable
    {
        PDFSettings Options { get; }
        DocumentBuilder Builder { get; }
        string exoRegularFile { get; set; }
        string exoItalicFile { get; set; }
        string exoBoldFile { get; set; }
        string FilePath { get; set; }
        Stream Stream { get; }
        int StartingPage { get; set; }
        IStreamCoordinator Coordinator { get; set; }
        void Write<T>(T data) where T : IEntity;
        void Write<T>(IEnumerable<T> data) where T : IEntity;
        void Write(IDictionary<string, object> data);
        void Flush();
    }
}