﻿using System;
using System.Globalization;
using System.Web.Mvc;

namespace responseTip
{
    public class DecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            /*            ValueProviderResult valueResult = bindingContext.ValueProvider
                            .GetValue(bindingContext.ModelName);

                        ModelState modelState = new ModelState { Value = valueResult };

                        object actualValue = null;

                        if (valueResult.AttemptedValue != string.Empty)
                        {
                            try
                            {
            //                    actualValue = Convert.ToDecimal(valueResult.AttemptedValue, CultureInfo.CurrentCulture);
                                actualValue = Convert.ToDecimal(valueResult.AttemptedValue,CultureInfo.CurrentCulture);
                            }
                            catch (FormatException e)
                            {
                                modelState.Errors.Add(e);
                            }
                        }


                        bindingContext.ModelState.Add(bindingContext.ModelName, modelState);

                        return actualValue;*/

            ValueProviderResult valueResult = bindingContext.ValueProvider
            .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                actualValue = Convert.ToDecimal(valueResult.AttemptedValue,
                    CultureInfo.CurrentCulture);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}