const { generateSwaggerTypes } = require("swagger-generator-codie");
const json = require("./swagger.json");

console.log("entrei");
generateSwaggerTypes(json, true);
