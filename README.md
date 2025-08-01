# Inventory Management System

## Simple & Effective Stock Control

<p align="center">
  <img src="./InventoryLogo.png" alt="Inventory Logo" width="250">
</p>
> ⭐ **Give a Star & Fork this project** ... Happy coding!

The **Inventory Management System** by TeAM is designed to handle
warehouse operations, stock movements, and product management efficiently.

Built with:
- **Backend:**
  - .NET Core API
  - **ORM:** Dapper & Entity Framework (EF)
  - **API Gateway:** Ocelot
  - **Message Queue:** RabbitMQ + MassTransit
  - **Real-time:** SignalR
  - **Code Generation:** Stored Procedures (Generated Code in SQL)
  - 
- **Frontend:**
  - React TypeScript
  - TanStack Form/Table
  - Ant Design (AntD)
  - 
- **Database:**
  - SQL Server
  - Azure SQL Database
 
  - **Deployment & Infrastructure:**
  - Docker (Containerization)
  - Kubernetes (Orchestration)
---

## 📌 Project Roadmap

### ✅ **Phase 1: Warehouse Management**
- Core modules for managing warehouses and storage bins.
- CRUD for products, stock, and business partners.
- Inventory transactions: Goods Receipt, Goods Issue, and Stock Transfer.
- Real-time **Current Stock** updates with audit logging.

### 🔒 **Phase 2: Authentication & Role-Based Access**
- Secure login system with user management.
- Role-based permission control (Admin, Warehouse Manager, Staff).
- Integration with `UserAccount`, `Role`, `Permission` tables.
- Enforce access control on critical inventory actions.

### 🛒 **Phase 3: E-Commerce & Sales**
- Product catalog for online sales.
- Order management and integration with inventory stock levels.
- Synchronization between warehouse transactions and e-commerce orders.
- Frontend shopping interface and checkout flow.

---

## ⚠️ SECURITY WARNING for Developers & Users

We value the security of this project. Please follow these guidelines:

✅ Only use official versions of this repository from **GitHub**:  
👉 [Inventory Management Repo](https://github.com/NowNotGay/InventoryManagement)

✅ All official updates and patches will be provided here.  

🚫 **DO NOT trust unknown versions** sent via email or from third-party sites.  
They may be **fake, malicious**, or intended to compromise data integrity.

✅ Always verify sources and contact the maintainer via email if in doubt:  
📧 **hnguyenngoc.h@gmail.com**

---

## Features

- 📦 **Warehouse Management**
- 🏷️ **Product Management**
- 🔄 **Inventory Transactions**
- 🏢 **Business Partner Management**
- 👥 **User & Role Management**
- 🛒 **E-Commerce & Sales Integration**
- 📊 **Audit & Reporting**

---

## Getting Started

1. **Clone the repository**
2. **Set up SQL Server database using `InventoryManagementDB.sql`**
3. **Configure backend connection string in `.NET appsettings.json`**
4. **Run Backend Server**:
   ```bash
   dotnet run
