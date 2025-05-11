# RestaurantMVC

A simple MVC-based restaurant management web application built with ASP.NET Core MVC. This project demonstrates the implementation of CRUD operations, user authentication, and role-based access control in a restaurant context.

## Features

* **Menu Management**: Add, edit, and delete menu items.
* **Order Processing**: Create and manage customer orders.
* **User Authentication**: Secure login and registration system.
* **Role-Based Access Control**: Different access levels for admins and staff.
* **Responsive Design**: Mobile-friendly interface using Bootstrap.

## Technologies Used

* ASP.NET Core MVC
* Entity Framework Core
* SQL Server
* Bootstrap
* Identity Framework

## Getting Started

### Prerequisites

* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
* [Visual Studio 2019 or later](https://visualstudio.microsoft.com/)

### Installation

1. **Clone the repository**:

   ```bash
   git clone https://github.com/SirTebz/ResturantMVC.git
   cd ResturantMVC
   ```

2. **Configure the database**:

   * Update the `appsettings.json` file with your SQL Server connection string.

3. **Apply migrations and seed the database**:

   ```bash
   dotnet ef database update
   ```

4. **Run the application**:

   ```bash
   dotnet run
   ```

   Navigate to `https://localhost:5001` in your browser.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
