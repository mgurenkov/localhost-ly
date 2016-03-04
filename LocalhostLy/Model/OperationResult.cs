using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections;
using ThePlatform.Common;

namespace ThePlatform.Common.Execution
{
    public class OperationResult
    {
        public IList<ValidationError> Errors { get; private set; }
        
        public Dictionary<string, object> Data { get; set; }
        public string Message { get; set; }

        public OperationResult()
        {
            Errors = new List<ValidationError>();
            Data = new Dictionary<string, object>();
        }

        public void AddError(string a_Message)
        {
            Add(null, a_Message);
        }

        public void AddError(string a_Field, string a_Message)
        {
            Add(a_Field, a_Message);
        }

        public bool HasErrors { get { return Errors.Count > 0; } }
        public bool OK { get { return Errors.Count == 0 && !Cancelled; } }
        public string AllErrors { get { return string.Join(Environment.NewLine, Errors.Select(x => string.Format("— {0}", x.Message)).Distinct()); } }
        public IList<ValidationError> FilteredErrors
        {
            get
            {
                var set = new HashSet<string>();
                var result = new List<ValidationError>();
                foreach (var error in Errors)
                {
                    if (string.IsNullOrWhiteSpace(error.Field))
                    {
                        result.Add(error);
                        continue;
                    }

                    if (set.Contains(error.Field)) continue;

                    set.Add(error.Field);
                    result.Add(error);
                }

                return result;
            }
        }
        public bool Cancelled { get; private set; }

        internal void Add(string a_Field, string a_Message)
        {
            Errors.Add(new ValidationError { Field = a_Field, Message = a_Message });
        }

        public void Cancel()
        {
            Cancelled = true;
        }
    }

    public class ValidationError
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }    
}
