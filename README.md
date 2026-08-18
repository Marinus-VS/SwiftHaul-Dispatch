# SwiftHaul Dispatch

A console-based fleet and cargo management system built in C# for PRG2781. SwiftHaul Dispatch simulates a courier company managing a mixed fleet of vehicles, tracking cargo, assigning loads, monitoring live dispatch events on a background thread, and saving/loading system state to disk.

### Domain Rule
* A vehicle cannot be assigned more cargo than its `VehicleCapacity` allows. 
* Every assignment checks the vehicle's current total assigned weight against its capacity before allowing a new item to be added. 
* The system blocks the operation with a `VehicleOverloadException` if the assignment would exceed it.

### Vehicle Types
| Type | Description | Capacity Range |
| :--- | :--- | :--- |
| **Wasp Runner** | Motorbike courier — fast, single-parcel deliveries | 1 – 30 kg |
| **Cascade Van** | Standard multi-stop delivery van | 30 – 1,700 kg |
| **Titan Hauler** | Heavy truck, supports 0–2 trailers | 1,000 – 36,000 kg |
| **Glacier Trans** | Refrigerated truck for temperature-controlled cargo | 30 – 7,000 kg |

### Cargo Types
| Type | Description | Weight Range |
| :--- | :--- | :--- |
| **Small Cargo** | Small parcels, optionally fragile | 1 – 30 kg |
| **Medium Cargo** | Standard packages, optionally requiring a signature | 30 – 1,000 kg |
| **Large Cargo** | Bulk cargo, optionally requiring a forklift | 1,000 – 36,000 kg |
| **Refrigerated Cargo** | Temperature-sensitive cargo with a required temperature | 30 – 7,000 kg |

### How to Run the Application
1. Open `SwiftHaul Dispatch.sln` in Visual Studio[cite: 2].
2. Restore NuGet packages if prompted (the project depends on `Newtonsoft.Json` for save/load functionality)[cite: 2].
3. Build the solution (Ctrl+Shift+B)[cite: 2].
4. Run with Ctrl+F5 (or F5 to debug)[cite: 2].
5. Navigate the console menus using the numbered options shown on screen[cite: 2].

*On first run, the fleet and cargo lists are empty[cite: 2]. Use **Manage Vehicles** and **Manage Cargo** to populate the system, or use **Manage Save Operations → Load Saved State** to load a previously saved loadout[cite: 2].*

### Menu Structure
```text
Main Menu
├── 1. Manage Vehicles       (Add / Display / Remove)[cite: 2]
├── 2. Manage Cargo          (Add / Display / Remove)[cite: 2]
├── 3. Assign Cargo to Vehicle[cite: 2]
├── 4. View Dispatch Log[cite: 2]
├── 5. Manage Save Operations (Save / Load / View / Remove / Clear)[cite: 2]
└── 6. Exit[cite: 2]
