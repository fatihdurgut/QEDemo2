# Pubs Database Schema Design

## Database Overview
The Pubs database is a sample database commonly used for learning SQL Server concepts. It represents a fictional book publishing company with information about authors, publishers, titles, sales, and employees.

## Tables List
The Pubs database contains the following **11 tables**:

1. **dbo.authors** - Author information
2. **dbo.discounts** - Discount information for stores
3. **dbo.employee** - Employee information
4. **dbo.jobs** - Job descriptions and salary ranges
5. **dbo.pub_info** - Publisher information and logos
6. **dbo.publishers** - Publisher details
7. **dbo.roysched** - Royalty schedules for titles
8. **dbo.sales** - Sales transactions
9. **dbo.stores** - Store information
10. **dbo.titleauthor** - Many-to-many relationship between titles and authors
11. **dbo.titles** - Book/title information

## Detailed Table Structures

### 1. authors
**Purpose**: Stores information about book authors.

| Column Name | Data Type | Nullable | Default | Description |
|------------|-----------|----------|---------|-------------|
| au_id | varchar(11) | NO | - | **Primary Key** - Author ID |
| au_lname | varchar(40) | NO | - | Author's last name |
| au_fname | varchar(20) | NO | - | Author's first name |
| phone | char(12) | NO | 'UNKNOWN' | Author's phone number |
| address | varchar(40) | YES | - | Author's address |
| city | varchar(20) | YES | - | Author's city |
| state | char(2) | YES | - | Author's state |
| zip | char(5) | YES | - | Author's ZIP code |
| contract | bit | NO | - | Contract status |

**Primary Key**: au_id

---

### 2. discounts
**Purpose**: Stores discount information for different stores.

| Column Name | Data Type | Nullable | Default | Description |
|------------|-----------|----------|---------|-------------|
| discounttype | varchar(40) | NO | - | Type of discount |
| stor_id | char(4) | YES | - | **Foreign Key** - Store ID |
| lowqty | smallint | YES | - | Low quantity threshold |
| highqty | smallint | YES | - | High quantity threshold |
| discount | decimal(4,2) | NO | - | Discount percentage |

**Foreign Keys**: 
- stor_id → stores.stor_id

---

### 3. employee
**Purpose**: Stores employee information.

| Column Name | Data Type | Nullable | Default | Description |
|------------|-----------|----------|---------|-------------|
| emp_id | char(9) | NO | - | **Primary Key** - Employee ID |
| fname | varchar(20) | NO | - | First name |
| minit | char(1) | YES | - | Middle initial |
| lname | varchar(30) | NO | - | Last name |
| job_id | smallint | NO | 1 | **Foreign Key** - Job ID |
| job_lvl | tinyint | YES | 10 | Job level |
| pub_id | char(4) | NO | '9952' | **Foreign Key** - Publisher ID |
| hire_date | datetime | NO | getdate() | Hire date |

**Primary Key**: emp_id
**Foreign Keys**: 
- job_id → jobs.job_id
- pub_id → publishers.pub_id

---

### 4. jobs
**Purpose**: Stores job descriptions and salary ranges.

| Column Name | Data Type | Nullable | Default | Description |
|------------|-----------|----------|---------|-------------|
| job_id | smallint | NO | - | **Primary Key** - Job ID |
| job_desc | varchar(50) | NO | 'New Position - title not formalized yet' | Job description |
| min_lvl | tinyint | NO | - | Minimum salary level |
| max_lvl | tinyint | NO | - | Maximum salary level |

**Primary Key**: job_id

---

### 5. pub_info
**Purpose**: Stores additional publisher information including logos.

| Column Name | Data Type | Nullable | Default | Description |
|------------|-----------|----------|---------|-------------|
| pub_id | char(4) | NO | - | **Primary Key & Foreign Key** - Publisher ID |
| logo | image | YES | - | Publisher logo |
| pr_info | text | YES | - | Publisher information |

**Primary Key**: pub_id
**Foreign Keys**: 
- pub_id → publishers.pub_id

---

### 6. publishers
**Purpose**: Stores publisher information.

| Column Name | Data Type | Nullable | Default | Description |
|------------|-----------|----------|---------|-------------|
| pub_id | char(4) | NO | - | **Primary Key** - Publisher ID |
| pub_name | varchar(40) | YES | - | Publisher name |
| city | varchar(20) | YES | - | City |
| state | char(2) | YES | - | State |
| country | varchar(30) | YES | 'USA' | Country |

**Primary Key**: pub_id

---

### 7. roysched
**Purpose**: Stores royalty schedules for titles.

| Column Name | Data Type | Nullable | Default | Description |
|------------|-----------|----------|---------|-------------|
| title_id | varchar(6) | NO | - | **Foreign Key** - Title ID |
| lorange | int | YES | - | Low sales range |
| hirange | int | YES | - | High sales range |
| royalty | int | YES | - | Royalty percentage |

**Foreign Keys**: 
- title_id → titles.title_id

---

### 8. sales
**Purpose**: Stores sales transaction information.

| Column Name | Data Type | Nullable | Default | Description |
|------------|-----------|----------|---------|-------------|
| stor_id | char(4) | NO | - | **Primary Key & Foreign Key** - Store ID |
| ord_num | varchar(20) | NO | - | **Primary Key** - Order number |
| ord_date | datetime | NO | - | Order date |
| qty | smallint | NO | - | Quantity sold |
| payterms | varchar(12) | NO | - | Payment terms |
| title_id | varchar(6) | NO | - | **Primary Key & Foreign Key** - Title ID |

**Primary Key**: stor_id, ord_num, title_id (Composite)
**Foreign Keys**: 
- stor_id → stores.stor_id
- title_id → titles.title_id

---

### 9. stores
**Purpose**: Stores bookstore information.

| Column Name | Data Type | Nullable | Default | Description |
|------------|-----------|----------|---------|-------------|
| stor_id | char(4) | NO | - | **Primary Key** - Store ID |
| stor_name | varchar(40) | YES | - | Store name |
| stor_address | varchar(40) | YES | - | Store address |
| city | varchar(20) | YES | - | City |
| state | char(2) | YES | - | State |
| zip | char(5) | YES | - | ZIP code |

**Primary Key**: stor_id

---

### 10. titleauthor
**Purpose**: Junction table for many-to-many relationship between titles and authors.

| Column Name | Data Type | Nullable | Default | Description |
|------------|-----------|----------|---------|-------------|
| au_id | varchar(11) | NO | - | **Primary Key & Foreign Key** - Author ID |
| title_id | varchar(6) | NO | - | **Primary Key & Foreign Key** - Title ID |
| au_ord | tinyint | YES | - | Author order |
| royaltyper | int | YES | - | Royalty percentage |

**Primary Key**: au_id, title_id (Composite)
**Foreign Keys**: 
- au_id → authors.au_id
- title_id → titles.title_id

---

### 11. titles
**Purpose**: Stores book/title information.

| Column Name | Data Type | Nullable | Default | Description |
|------------|-----------|----------|---------|-------------|
| title_id | varchar(6) | NO | - | **Primary Key** - Title ID |
| title | varchar(80) | NO | - | Book title |
| type | char(12) | NO | 'UNDECIDED' | Book type/category |
| pub_id | char(4) | YES | - | **Foreign Key** - Publisher ID |
| price | money | YES | - | Book price |
| advance | money | YES | - | Author advance |
| royalty | int | YES | - | Royalty percentage |
| ytd_sales | int | YES | - | Year-to-date sales |
| notes | varchar(200) | YES | - | Notes |
| pubdate | datetime | NO | getdate() | Publication date |

**Primary Key**: title_id
**Foreign Keys**: 
- pub_id → publishers.pub_id

---

## Entity Relationship Diagram (ERD) Description

### Primary Relationships:

1. **Publishers → Titles** (One-to-Many)
   - One publisher can publish many titles

2. **Publishers → Employees** (One-to-Many)
   - One publisher can have many employees

3. **Publishers → Pub_Info** (One-to-One)
   - Each publisher has additional info

4. **Jobs → Employees** (One-to-Many)
   - One job type can have many employees

5. **Titles → TitleAuthor** (One-to-Many)
   - One title can have multiple authors

6. **Authors → TitleAuthor** (One-to-Many)
   - One author can write multiple titles

7. **Titles → Sales** (One-to-Many)
   - One title can have multiple sales records

8. **Stores → Sales** (One-to-Many)
   - One store can have multiple sales records

9. **Stores → Discounts** (One-to-Many)
   - One store can have multiple discount schemes

10. **Titles → RoyaltySchedules** (One-to-Many)
    - One title can have multiple royalty tiers

### Key Business Rules:

- Authors and Titles have a many-to-many relationship (via TitleAuthor table)
- Each sale record represents a specific title sold at a specific store
- Employees belong to publishers and have specific job roles
- Royalty schedules define different royalty rates based on sales ranges
- Discounts are store-specific and can vary by quantity ranges

## Views

### titleview
A view that combines title, author, and sales information:

| Column Name | Data Type | Description |
|------------|-----------|-------------|
| title | varchar(80) | Book title |
| au_ord | tinyint | Author order |
| au_lname | varchar(40) | Author last name |
| price | money | Book price |
| ytd_sales | int | Year-to-date sales |
| pub_id | char(4) | Publisher ID |

---

*Schema generated on: October 14, 2025*
*Database: Pubs on FATIH-PC2\SQLEXPRESS*