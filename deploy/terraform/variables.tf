variable "environment" {
  description = "Environment name (dev, staging, prod)"
  type        = string
  default     = "dev"
}

variable "location" {
  description = "Azure region for resources"
  type        = string
  default     = "eastus"
}

variable "resource_group_name" {
  description = "Name of the resource group"
  type        = string
  default     = "pubs-microservices-rg"
}

variable "aks_cluster_name" {
  description = "Name of the AKS cluster"
  type        = string
  default     = "pubs-aks-cluster"
}

variable "node_count" {
  description = "Number of nodes in the AKS cluster"
  type        = number
  default     = 3
}

variable "vm_size" {
  description = "Size of the virtual machines in the node pool"
  type        = string
  default     = "Standard_D2s_v3"
}

variable "sql_server_name" {
  description = "Name of the Azure SQL Server"
  type        = string
  default     = "pubs-sql-server"
}

variable "sql_database_name" {
  description = "Name of the SQL database"
  type        = string
  default     = "PubsDb"
}

variable "sql_admin_username" {
  description = "Admin username for SQL Server"
  type        = string
  default     = "sqladmin"
}

variable "sql_admin_password" {
  description = "Admin password for SQL Server"
  type        = string
  sensitive   = true
}

variable "redis_cache_name" {
  description = "Name of the Azure Redis Cache"
  type        = string
  default     = "pubs-redis-cache"
}

variable "redis_sku_name" {
  description = "SKU name for Redis Cache"
  type        = string
  default     = "Basic"
}

variable "redis_family" {
  description = "Redis cache family"
  type        = string
  default     = "C"
}

variable "redis_capacity" {
  description = "Redis cache capacity"
  type        = number
  default     = 1
}

variable "tags" {
  description = "Tags to apply to resources"
  type        = map(string)
  default = {
    Project     = "Pubs Microservices"
    ManagedBy   = "Terraform"
  }
}
