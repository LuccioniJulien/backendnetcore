using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BaseApi.Helper {
    public static class Format {
        public static object ToBadRequest (this ModelStateDictionary modelState, int statut = 400) {
            List<string> errors = modelState.Keys
                .SelectMany (key => modelState[key].Errors.Select (x => x.ErrorMessage))
                .ToList ();

            var response = new {
                meta = new {
                errors,
                statut
                }
            };

            return response;
        }

        public static object ToBadRequest (this string message, int status = 400) {
            string[] errors = { message };

            var response = new {
                meta = new {
                errors,
                status
                }
            };

            return response;
        }

        public static object ToMessage<T> (T data, int status) => new { data, meta = new { status } };

        public static object ToMessage<T> (T data, int status, object token = null) => new { data, meta = new { token, status } };

    }
}