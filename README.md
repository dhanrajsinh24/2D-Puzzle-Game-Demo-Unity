
# Connect Flow

**Connect Flow** is a puzzle game inspired by **Loop Energy** game (`https://play.google.com/store/apps/details?id=com.infinitygames.loopenergy`) where the objective is to connect all computers to WiFi using available nodes, either directly or through cables. Once all computers are connected, the level is cleared, and the player’s score is calculated based on the number of moves made.

## Features

- **Levels:** The game includes 4 levels.
- **Simple Controls:** Touch to rotate the nodes clockwise.
- **Scoring:** The score is calculated based on the number of moves taken to complete a level.
- **Visual Feedback:** Nodes change color to indicate a successful connection to the WiFi.
- **Camera Effects:** The camera shakes upon clearing a level.
- **Background Music:** Continuous background music.
- **Addressable Assets:** Levels are loaded dynamically using Unity's Addressable Assets system.
- **Data Storage:** Level data and score are stored using JSON files.
- **Ring Effect:** Triggered at the start and end of each level.
- **Particle Effects:** Active during gameplay to enhance visual appeal.

## Technical Details

- **Unity Version:** 2022.3.27f1
- **Programming Language:** C#
- **Main Scene:** The primary game scene is named `Game`.
- **Addressable Assets:**: Used for efficient level loading.
- **JSON Storage:**: For storing level and score data.
- **Scene Management:**: The main gameplay takes place in the Game scene.

## Main Scripts Overview
The project includes several custom scripts, each serving a specific purpose in the game:

- **Node.cs**: Represents a node in the grid, including its connection status and rotation logic.
- **WiFiNode.cs**: Extends Node to specifically manage the behavior and connections of the WiFi node.
- **ComputerNode.cs**: Extends Node to manage the behavior and connection requirements of computer nodes.
- **CableNode.cs**: Extends Node to handle the behavior of cable nodes that connect computers to the WiFi node.
- **RotatableNode.cs**: Handles the rotation logic of nodes and checks for connections upon rotation.
- **CircuitValidation.cs**: Validates the circuit to ensure all computers are connected to WiFi and triggers level completion.
- **LevelManager.cs**: Manages level loading, progression, and score calculation.
- **SpriteGridManager.cs**: Initializes and manages the grid layout using Unity’s Grid component.
- **UIManager.cs**: Managers communication between all UI scripts and manager scripts.
- **CameraShake.cs**: Implements the camera shake effect triggered on level completion.
- **DatabaseManager.cs**: Handles Json database management for level progress and score.
- **NodeClickManager.cs**: Responsible for node clicking start, stop and notify node of clicking.
- **ScoreManager.cs**: Manages the scoring based on the number of moves taken to complete a level.
- **NodeFactory.cs**: Handles creating Various nodes. Attached to Grid object.
- **LevelConfig.cs**: Defines the structure and settings for each level.
- **LevelScrollView.cs**: Level scroll which handles all level lock / unlock visuals.
- **LevelButton.cs**: Defines the level button which is used in Level scroll view for level selection.


## Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/your-username/connect-flow.git
   ```

2. **Open in Unity:**
   - Open Unity Hub.
   - Add the cloned project.
   - Open the project with Unity 2022.3.27f1.

3. **Play the Game:**
   - Open the `Game` scene.
   - Press the Play button in Unity to start the game.

## How to Play

1. **Objective:**
   - Rotate the nodes to connect all computers to the WiFi, directly or through cables.

2. **Controls:**
   - **Rotate Nodes:** Touch on any node to rotate it clockwise.

3. **Scoring:**
   - Scores are calculated based on the number of moves used to complete a level.
