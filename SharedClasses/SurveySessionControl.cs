using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses {
    public class SurveySessionControl<T> {
        public T Value { get; set; }

        public SurveySessionControl( T value ) {
            Value = value;
        }
    }
}
