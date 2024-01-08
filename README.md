# Introducing FakeApi: A Versatile API for Testing and Development

In the world of software development, having access to reliable and versatile APIs for testing and development purposes is crucial. Developers often find themselves in need of mock APIs that can simulate real-world scenarios and provide the flexibility to work with different data formats. That's why I'm excited to introduce **FakeApi**, a feature-rich API service designed to meet the diverse needs of developers.

## Overview

**FakeApi** is a robust API service built using the .NET 8 framework (similar to .NET 5 or .NET 6) that addresses the limitations of existing public APIs. One of its standout features is its ability to support both JSON and XML responses, giving developers the flexibility they need for various testing and development scenarios.

### Key Features:

1. **Dual Response Format:**
   - **JSON:** Get responses in JSON format.
   - **XML:** Receive responses in XML format.

2. **Data Storage and Retrieval:**
   - The API has the capability to store modified data, allowing developers to work with custom datasets.
   - Responses are stored for each IP, making it easy to retrieve modified data for 30 minutes per user.

## Getting Started

### Service URL: [https://beerealm.com:7163/](https://beerealm.com:7163/)
### Swagger Endpoint: [https://beerealm.com:7163/swagger/index.html](https://beerealm.com:7163/swagger/index.html)

### Endpoints:

- **GET /api/v1/Todos:**
  - Retrieves a list of todos in both JSON and XML formats.

- **POST /api/v1/Todos:**
  - Adds a new todo. Accepts requests in JSON or XML format.

- **GET /api/v1/Todos/{id}:**
  - Retrieves a specific todo by ID.

- **DELETE /api/v1/Todos/{id}:**
  - Deletes a todo by ID.

- **GET /api/v1/Users:**
  - Retrieves a list of users along with their todos. Supports both JSON and XML.

- **POST /api/v1/Users:**
  - Adds a new user. Accepts requests in JSON or XML format.

- **GET /api/v1/Users/{id}:**
  - Retrieves a specific user by ID.

- **DELETE /api/v1/Users/{id}:**
  - Deletes a user by ID.

- **GET /api/v1/Users/{id}/todos:**
  - Retrieves todos for a specific user.

- **DELETE /api/v1/Users/{id}/todos/{todoId}:**
  - Deletes a todo for a specific user.

## Why FakeApi?

1. **Support for Both JSON and XML:**
   - Developers often need to work with different data formats. FakeApi allows you to choose between JSON and XML responses based on your preference.

2. **Data Persistence:**
   - FakeApi goes beyond traditional mock APIs by allowing developers to store modified data. Responses are stored per IP for 30 minutes, enabling efficient testing and development.

3. **.NET 8 Framework:**
   - Developed using the .NET 8 framework (similar to .NET 5 or .NET 6), FakeApi leverages the latest features and improvements for a seamless development experience.

## How to Use FakeApi

1. **Visit the Service URL:**
   - Access the FakeApi service at [https://beerealm.com:7163/](https://beerealm.com:7163/).

2. **Explore Endpoints:**
   - Check out the Swagger documentation at [https://beerealm.com:7163/swagger/index.html](https://beerealm.com:7163/swagger/index.html) for detailed information on available endpoints and their usage.

3. **Make Requests:**
   - Use your favorite HTTP client or tool to make requests to the FakeApi endpoints. Specify the desired content type (JSON or XML) in the request headers.

4. **Retrieve Modified Data:**
   - Take advantage of the data storage feature by retrieving modified data for each IP within the 30-minute window.

## Conclusion

FakeApi is a valuable addition to the developer's toolkit, providing a flexible and reliable solution for testing and development scenarios. Its support for both JSON and XML, coupled with data storage capabilities, sets it apart as a versatile choice for developers seeking a reliable mock API.

Visit [https://beerealm.com:7163/](https://beerealm.com:7163/) to explore FakeApi and streamline your testing and development workflows.

Happy coding! 🚀
