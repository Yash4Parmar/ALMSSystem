import * as yup from "yup";

export const removeFormFields = (FormSchema: Global.FormSchema, names: string[]): Global.FormSchema => {
  // Filter out fields with the specified names
  const updatedFields = FormSchema.fields.filter(field => !names.includes(field.name));

  // Remove the fields from validation schema
  const updatedValidationSchemaFields = Object.keys(FormSchema.validationSchema.fields)
      .filter(fieldName => !names.includes(fieldName))
      .reduce((acc, fieldName) => {
          acc[fieldName] = FormSchema.validationSchema.fields[fieldName];
          return acc;
      }, {});

  const updatedValidationSchema = yup.object(updatedValidationSchemaFields);

  // Remove the fields from default values
  const updatedDefaultValues = { ...FormSchema.defaultValues };
  names.forEach(name => {
      delete updatedDefaultValues[name];
  });

  return {
      ...FormSchema,
      fields: updatedFields,
      validationSchema: updatedValidationSchema,
      defaultValues: updatedDefaultValues
  };
};
