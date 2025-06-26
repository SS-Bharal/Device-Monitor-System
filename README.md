# 📡 Device Monitor System

**Device Monitor System** is a multi-component IoT simulation platform that enables device registration, real-time alarm generation, and alarm reporting. It consists of four projects:
a dashboard for users, an API service, a simulator for generating device data, and a service that saves data from a message queue.

---

## 🧩 Project Overview

### 1. 📊 Angular Dashboard (Frontend)
- Provides a web interface to **register devices** and **view alarm reports**.
- Validates form inputs and interacts with the API via HTTP.
- Displays real-time data of alarms triggered by each device.

---

### 2. 🌐 .NET Core API (Backend)
- Exposes REST APIs to **register devices**, **fetch device data**, and **store alarm entries**.
- Interacts with SQL Server to store device and alarm details.
- Acts as the main communication bridge between frontend, simulator, and DB.

---

### 3. 🧪 Simulator (.NET Core Console App)
- Simulates registered devices by **sending data every 5 seconds**.
- Triggers **one random alarm every 15 seconds** per device.
- Sends data to RabbitMQ queue using **Direct Exchange** format.

---

### 4. 💾 Data Save Service (.NET Core Console App)
- Consumes data from RabbitMQ.
- Saves incoming device alarm messages to the database.
- Ensures decoupled and reliable message processing.

---

## 🛠️ Tech Stack

- **Frontend**: Angular
- **Backend API**: .NET Core Web API + SQL Server
- **Message Queue**: RabbitMQ
- **Console Apps**: .NET Core (for Simulator and Consumer)

---

## 🧪 How It Works

1. Register devices through the Angular Dashboard.
2. The Simulator fetches registered devices and sends simulated alarm data to RabbitMQ.
3. The Data Save Service listens to the queue and stores the alarms in the database.
4. The Dashboard displays the live alarm report for each device.


---

## 👨‍💻 Author

**Sahil Bharal**  
🌐 [Portfolio](https://sahilbharal.website)  
💼 [GitHub](https://github.com/SS-Bharal)

---
