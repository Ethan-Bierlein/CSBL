using System;

namespace CSBL.Reporting
{
    public class Error
    {
        public ErrorStage Stage { get; set; }
        public ErrorType Type { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// Constructor for the Error class.
        /// </summary>
        /// <param name="stage">The execution the error happened at.</param>
        /// <param name="type">The type of error.</param>
        /// <param name="message">The error message.</param>
        public Error(ErrorStage stage, ErrorType type, string message)
        {
            this.Stage = stage;
            this.Type = type;
            this.Message = message;
        }

        /// <summary>
        /// Report the error to the console.
        /// </summary>
        /// <param name="errorFormattingArguments">The formatting arguments passed to the error message.</param>
        public void Report(params dynamic[] errorFormattingArguments)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[STAGE::{0}] [TYPE::{1}] ", this.Stage, this.Type);
            Console.Write(this.Message + "\n", errorFormattingArguments);
            Console.ResetColor();
        }
    }
}
