# Code of Conduct for the Inventory Management System Project

## Our Pledge

We, as members, developers, and leaders of the Inventory Management System project, pledge to foster a harassment-free environment for everyone, regardless of:

- Age
- Body size
- Visible or invisible disability
- Ethnicity
- Sex characteristics
- Gender identity and expression
- Level of experience
- Education
- Socio-economic status
- Nationality
- Personal appearance
- Race
- Religion
- Sexual identity and orientation

We commit to acting and interacting in ways that contribute to an open, welcoming, diverse, inclusive, and healthy community, supporting the effective development of the inventory management system.

## Our Standards

### Acceptable Behaviors

Behaviors that contribute to a positive environment include:

- Demonstrating empathy and kindness toward team members.
- Respecting differing opinions, viewpoints, and experiences, especially during discussions on system design, database structure (SQL), or user interface (React TypeScript, Ant Design).
- Providing and gracefully accepting constructive feedback, particularly for features like warehouse management (`Warehouse`), transactions (`InventoryTransaction`), or products (`Product`).
- Accepting responsibility, apologizing for mistakes, and learning from experiences to improve processes (e.g., optimizing Dapper queries or TanStack Table interfaces).
- Focusing on the project and community’s best interests, ensuring requirements like updating inventory (`CurrentStock`) or handling goods transactions are met.

### Unacceptable Behaviors

The following behaviors are not tolerated:

- Using sexualized language or imagery, or engaging in any form of sexual harassment.
- Trolling, insulting, derogatory comments, or personal/political attacks, especially in code or UI design discussions.
- Public or private harassment, including in project email, Slack, or GitHub.
- Publishing others’ private information (e.g., email, phone numbers) without explicit permission.
- Any conduct deemed inappropriate in a professional setting, such as neglecting development processes (e.g., untested code commits or failing to update `AuditLog`).

## Enforcement Responsibilities

Project leaders are tasked with:

- Clarifying and enforcing these standards of acceptable behavior.
- Removing, editing, or rejecting comments, commits, code, wiki edits, issues, or contributions (e.g., SQL scripts, .NET code, React components) that violate this Code.
- Communicating reasons for moderation decisions when appropriate, ensuring transparency.

## Scope

This Code applies to:

- All project spaces (e.g., GitHub, GitLab repositories).
- Internal communication channels (e.g., Slack, project email, online/offline meetings).
- Public spaces when representing the project (e.g., conference presentations, official social media accounts).

## Reporting and Enforcement

- **Reporting**: Instances of unacceptable behavior can be reported to project leaders at [hnguyenngoc.h@gmail.com]. All complaints will be reviewed and investigated promptly, fairly, and confidentially.
- **Privacy**: Project leaders must respect the reporter’s privacy and security.

## Enforcement Guidelines

Project leaders will follow these Community Impact Guidelines:

1. **Correction**
   - **Impact**: Inappropriate language or unprofessional behavior (e.g., committing code without proper Dapper usage or failing to update `CurrentStock` after transactions).
   - **Consequence**: Private written warning, clarifying the violation and requesting a public apology if needed.

2. **Warning**
   - **Impact**: Single or repeated violations (e.g., neglecting `InventoryTransaction` logs or disregarding UI feedback).
   - **Consequence**: Warning with a no-interaction period; violating this may lead to a ban.

3. **Temporary Ban**
   - **Impact**: Serious violations (e.g., corrupting `Warehouse` data or harassing team members).
   - **Consequence**: Temporary ban from interactions for a specified period; further violation may result in a permanent ban.

4. **Permanent Ban**
   - **Impact**: Pattern of violations (e.g., sustained harassment or compromising `CurrentStock` data).
   - **Consequence**: Permanent ban from all project interactions.

## Integration with the Inventory Management System

- **Action Logging**: Record all actions (e.g., `Warehouse`, `Product`, `InventoryTransaction`) in `AuditLog` for transparency.
- **Access Control**: Manage permissions via `Permission` and `UserRoleAssignment` to restrict sensitive actions.
- **User Interface**: Use React TypeScript (TanStack Form/Table, Ant Design) to display warnings for improper actions (e.g., transactions without `CurrentStock` updates).
- **Automated Code Generation**: Leverage `GenerateCodeByConfig` for consistent transaction codes (e.g., `GRN`, `GIN`, `STF`).
- **Phase 1: Warehouse Management**:
  - Ensure all warehouse-related operations (e.g., `Warehouse`, `StorageBin`, `CurrentStock`) are accurately implemented and logged, with proper UI feedback using TanStack Table for inventory tracking.
- **Phase 2: User Account Creation and Permission Management**:
  - Implement secure user registration and login via `UserAccount`, with role-based access control managed through `UserRoleAssignment` and `Permission`, ensuring Dapper queries are optimized for performance.
- **Phase 3: Ecommerce Shop Functionality**:
  - Integrate ecommerce features (e.g., product listings from `Product` and `ProductVariant`, order processing linked to `GoodsIssueNote`) with a responsive React TS frontend, ensuring real-time inventory updates in `CurrentStock`.

## Attribution

Not Yet
