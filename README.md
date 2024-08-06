
# Connect Flow

**Connect Flow** is a puzzle game where the objective is to connect all computers to WiFi using available nodes, either directly or through cables. Once all computers are connected, the level is cleared, and the player’s score is calculated based on the number of moves made.

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
