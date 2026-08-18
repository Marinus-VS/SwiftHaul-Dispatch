# SwiftHaul Dispatch

A console-based fleet and cargo management system built in C# for PRG2781. SwiftHaul Dispatch simulates a courier company managing a mixed fleet of vehicles, tracking cargo, assigning loads, monitoring live dispatch events on a background thread, and saving/loading system state to disk.

### Domain Rules
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

### Test Data & Persistence
The project includes a pre-configured dataset located at `Saves/TestData.json`:
* Contains 8 pre-populated vehicles (across all 4 vehicle types) and 16 diverse cargo items.
* Pre-configured with `Copy if newer` so it deploys automatically to the build output directory (`bin/Debug/Saves/` or `bin/Release/Saves/`).
* To load this dataset during testing, go to **Manage Save Operations $\rightarrow$ Load Saved State** and select `TestData.json`.

### How to Run the Application
1. Open `SwiftHaul Dispatch.sln` in Visual Studio.
2. Restore NuGet packages if prompted (the project depends on `Newtonsoft.Json` for save/load functionality).
3. Build the solution (`Ctrl+Shift+B`).
4. Run with `Ctrl+F5` (or `F5` to debug).
5. Navigate the console menus using the numbered options shown on screen.

*On first run without loading data, the fleet and cargo lists are empty. Use **Manage Vehicles** and **Manage Cargo** to populate the system manually, or use **Manage Save Operations $\rightarrow$ Load Saved State** to load `TestData.json`*

### Menu Structure
```text
Main Menu
├── 1. Manage Vehicles       (Add / Display / Remove)
├── 2. Manage Cargo          (Add / Display / Remove)
├── 3. Assign Cargo to Vehicle
├── 4. View Dispatch Log
├── 5. Manage Save Operations (Save / Load / View / Remove / Clear)
└── 6. Exit
