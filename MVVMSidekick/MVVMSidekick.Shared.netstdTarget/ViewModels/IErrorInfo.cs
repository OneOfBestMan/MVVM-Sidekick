using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MVVMSidekick.ViewModels
{
    /// <summary>
    /// Interface IErrorInfo
    /// </summary>
    public interface IErrorInfo
    {
        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>The errors.</value>
        ObservableCollection<ErrorEntity> Errors { get; }
    }
    /// <summary>
    /// Class ErrorEntity.
    /// </summary>
    public class ErrorEntity
    {
        public ErrorEntity()
        {

        }


        public string PropertyName { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; set; }
        /// <summary>
        /// Gets or sets the inner error information source.
        /// </summary>
        /// <value>The inner error information source.</value>
        public IErrorInfo InnerErrorInfoSource { get; set; }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {

            return null;// string.Format("{0}，{1}，{2}", Message, Exception, InnerErrorInfoSource);
        }
    }
}
