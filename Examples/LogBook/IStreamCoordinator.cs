using System;
using System.Collections.Generic;
using Gehtsoft.PDFFlow.LogBook.Model;

namespace Gehtsoft.PDFFlow.LogBook
{
    public interface IStreamCoordinator:IDisposable
    {
        void Input<T>(T data) where T:IEntity;
        void Input<T>(IEnumerable<T> data) where T:IEntity;
        void Input(IDictionary<string, object> data);
    }
}