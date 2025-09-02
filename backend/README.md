# Backend API for Leão Adv

This project is a backend API for the Leão Adv application, built using FastAPI. It follows a clean architecture with a layered structure, ensuring maintainability and scalability.

## Project Structure

```
backend
├── src
│   ├── main.py                # Entry point of the FastAPI application
│   ├── api
│   │   ├── routes.py          # Main application routes
│   │   └── auth_routes.py      # Authentication routes
│   ├── controllers
│   │   ├── user_controller.py  # User-related request handling
│   │   └── oracle_controller.py # Oracle feature request handling
│   ├── services
│   │   ├── user_service.py     # Business logic for user management
│   │   └── oracle_service.py    # Business logic for OpenAI API interaction
│   ├── repositories
│   │   └── user_repository.py   # Data access for user operations
│   ├── models
│   │   └── user_model.py        # User model definition
│   ├── schemas
│   │   ├── user_schema.py       # User data validation and serialization
│   │   └── oracle_schema.py      # Oracle feature data validation and serialization
│   ├── utils
│   │   └── auth.py              # Authentication utility functions
│   ├── config
│   │   └── settings.py          # Configuration settings
│   └── integrations
│       └── openai_client.py     # OpenAI client setup and functions
├── requirements.txt             # Project dependencies
└── README.md                    # Project documentation
```

## Setup Instructions

1. **Clone the repository:**
   ```
   git clone <repository-url>
   cd backend
   ```

2. **Create a virtual environment:**
   ```
   python -m venv venv
   source venv/bin/activate  # On Windows use `venv\Scripts\activate`
   ```

3. **Install dependencies:**
   ```
   pip install -r requirements.txt
   ```

4. **Configure environment variables:**
   Create a `.env` file in the `backend` directory and add your configuration settings, such as database connection strings and API keys.

5. **Run the application:**
   ```
   uvicorn src.main:app --reload
   ```

## Usage Examples

- **User Registration:**
  - Endpoint: `POST /auth/register`
  - Body: `{ "username": "example", "password": "example" }`

- **User Login:**
  - Endpoint: `POST /auth/login`
  - Body: `{ "username": "example", "password": "example" }`

- **Query Oracle:**
  - Endpoint: `POST /oracle/query`
  - Body: `{ "question": "What is intellectual property?" }`

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any enhancements or bug fixes.

## License

This project is licensed under the MIT License. See the LICENSE file for more details.