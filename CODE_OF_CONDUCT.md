Code of Conduct for the Inventory Management System Project
Our Pledge
We, as members, developers, and leaders of the Inventory Management System project, pledge to foster a harassment-free environment for everyone, regardless of age, body size, visible or invisible disability, ethnicity, sex characteristics, gender identity and expression, level of experience, education, socio-economic status, nationality, personal appearance, race, religion, or sexual identity and orientation.
We commit to acting and interacting in ways that contribute to an open, welcoming, diverse, inclusive, and healthy community, supporting the effective development of the inventory management system.
Our Standards
Behaviors that contribute to a positive environment in our project include:

Demonstrating empathy and kindness toward other team members.
Respecting differing opinions, viewpoints, and experiences, especially during discussions about system design, database structure (SQL), or user interface (React TypeScript, Ant Design).
Providing and gracefully accepting constructive feedback, particularly when working on features like warehouse management (Warehouse), transactions (InventoryTransaction), or products (Product).
Accepting responsibility and apologizing for mistakes, learning from experiences to improve development processes, such as optimizing Dapper queries or TanStack Table interfaces.
Focusing on what is best for the project and community, not just individual interests, ensuring the system meets requirements like updating inventory (CurrentStock) or handling goods receipt/issue/transfer transactions.

Unacceptable behaviors include:

Using sexualized language or imagery, or engaging in any form of sexual harassment.
Trolling, making insulting or derogatory comments, or launching personal or political attacks, particularly in discussions about code or UI design.
Engaging in public or private harassment, including in communication channels like project email, Slack, or GitHub.
Publishing others’ private information, such as email addresses or phone numbers, without explicit permission.
Other conduct deemed inappropriate in a professional setting, such as failing to follow development processes (e.g., committing untested code or neglecting AuditLog updates).

Enforcement Responsibilities
Project leaders are responsible for clarifying and enforcing these standards of acceptable behavior. They have the right and responsibility to:

Remove, edit, or reject comments, commits, code, wiki edits, issues, or other contributions (e.g., SQL scripts, .NET code, or React components) that do not align with this Code of Conduct.
Communicate reasons for moderation decisions when appropriate, ensuring transparency in actions related to the project.

Scope
This Code of Conduct applies to all project spaces, including:

Code repositories (e.g., GitHub, GitLab, or similar platforms).
Internal communication channels such as Slack, project email, or online/offline meetings.
Public spaces when representing the project, such as presenting the inventory management system at conferences or using official project accounts on social media.

Reporting and Enforcement
Instances of abusive, harassing, or otherwise unacceptable behavior may be reported to the project leaders at [project leader’s email, e.g., inventorymanagement@company.com]. All complaints will be reviewed and investigated promptly, fairly, and confidentially.
Project leaders are obligated to respect the privacy and security of the reporter of any incident.
Enforcement Guidelines
Project leaders will follow these Community Impact Guidelines to determine consequences for actions violating this Code of Conduct:

Correction  

Impact: Use of inappropriate language or unprofessional behavior, such as committing code that does not follow conventions (e.g., incorrect Dapper usage or failing to update CurrentStock after transactions).  
Consequence: A private, written warning from project leaders, clarifying the nature of the violation and explaining why the behavior was inappropriate. A public apology may be requested.


Warning  

Impact: A violation through a single incident or series of actions, such as repeatedly neglecting to log transactions (InventoryTransaction) or disregarding feedback on UI design (Ant Design).  
Consequence: A warning with consequences for continued behavior, prohibiting interaction with involved individuals, including those enforcing the Code of Conduct, for a specified period. Violating these terms may lead to a temporary or permanent ban.


Temporary Ban  

Impact: A serious violation of community standards, such as intentionally corrupting warehouse data (Warehouse, StorageBin) or harassing team members.  
Consequence: A temporary ban from all interactions or public communication with the project for a specified period. No public or private interactions with involved individuals are allowed during this time. Violating these terms may lead to a permanent ban.


Permanent Ban  

Impact: A pattern of violating community standards, including sustained inappropriate behavior, harassment of individuals, or actions that compromise inventory data (CurrentStock).  
Consequence: A permanent ban from all public interactions within the project.



Integration with the Inventory Management System
To ensure effective application of this Code of Conduct, the following measures will be implemented:

Action Logging: All significant actions (create, update, delete) in the system, including changes to tables like Warehouse, Product, or InventoryTransaction, must be recorded in the AuditLog table to ensure transparency and accountability.
Access Control: User permissions are managed via the Permission and UserRoleAssignment tables, ensuring only authorized users can perform sensitive actions like updating inventory or creating transactions (GoodsReceiptNote, GoodsIssueNote, StockTransfer).
User Interface: The React TypeScript interface (using TanStack Form/Table and Ant Design) will display notifications or warnings for inappropriate actions, such as attempting to create transactions without updating CurrentStock.
Automated Code Generation: The stored procedure GenerateCodeByConfig ensures consistent and automatic generation of transaction codes (e.g., GRN, GIN, STF), minimizing human error.

Attribution
This Code of Conduct is adapted from the Contributor Covenant, version 2.0, available at https://www.contributor-covenant.org/version/2/0/code_of_conduct.html. Community Impact Guidelines were inspired by Mozilla’s code of conduct enforcement ladder.
For answers to common questions, see the FAQ at https://www.contributor-covenant.org/faq. Translations are available at https://www.contributor-covenant.org/translations.
