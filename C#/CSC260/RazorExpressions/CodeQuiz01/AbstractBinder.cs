using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeQuiz01 {
    public class AbstractBinder : DefaultModelBinder {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType) {
            try {
                return base.CreateModel(controllerContext, bindingContext, modelType);
            } catch {
                try {
                    return Activator.CreateInstance(modelType);
                } catch {
                    return null;
                }
            }
        }
    }
}