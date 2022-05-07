# Evo-V3-Unity
##### Made by: Shahar Yair
### A genetic algorithm built on the unity platform to allow simulating evolution of organisms

##### This repo is a separate continuance of: [Playing-With-Spheres](https://github.com/Shahar-Y/Playing-With-Spheres)

##### 3rd Version. Previous versions can be found at: [V2+V1](https://github.com/Shahar-Y/Mutation)

### To run the alorithm, clone the repository and run as a unity project.

#### Requirements:

- Unity version 2021.3.0f1
- 8 GB RAM

![Simulation Gif](https://media.giphy.com/media/lgodyuxB7q1QK1Jkmb/giphy.gif)

### While the simulation is running, you can travel and click on cells to get their current real-time data!

#### Travel using (Right-Click + WASD).

#### Speed-up by pressing (Shift).

#### Get data by clicking on the cells (Left-Click).

## Output:

#### You may stop the simulation at any time.

#### The simulation streams the data in real-time to a csv file in its folder with the starting timestamp.

#### Example output of the simulation data converted to graph:

![evo-v3-stats](./ReadmeFolder/evo-v3-stats.png)

### Cells Parameters:
 - Speed: the speed in which the cell will move. Defined by: `Globals.SpeedUnits` * `cell.SpeedPoints`.
 - SightRadius: the sight radius in which the cell can see. Defined by: `Globals.SightRadiusUnits` * `cell.SightRadiusPoints`.
 - MaxFullness: maximum fullness the cell can reach. Defined by: `Globals.MaxFullnessUnits` * `cell.MaxFullnessPoints`.
 - Size: the size of the cell. Defined by: `Globals.SizeUnits` * `cell.SizePoints`.
 - FoodWorth: worth of each food unit consumed. Defined by: `Globals.FoodWorthUnits` * `cell.FoodWorthPoints`.
 - ReplicationRate: the rate of replication of the cell. In ths version - is constant. Defined by: `Globals.ReplicationRateUnits` * `cell.ReplicationRatePoints`.



### Cells Rules:

1. A cell will look for food as long as it is hungry.
2. A cell is hungry if eating a single unit of food will make its fullness equal or lower than `MaxFullness`.
3. A cell can only see in its `SightRadius`.
4. A cell will always move at its `Speed`. 
5. Replication: a cell will divide when two conditions are met:  
  5.1 The `TimeToReplication` (starting at `ReplicationRate`) has reached 0.  
  5.2 `Fullness` > `cell.CellMaxFullness` / 2.
6. When a cell divides, its `Fullness` is cut by half and its `TimeToReplication` is reset to `cell.ReplicationRate`.
7. Mutation: On each replication, there is a chance of mutation: increasing one parameter's replication point with another's (increasing one and decreasing the other).




