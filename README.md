# HAL
## Unity-based Ros Visualizer
Binary executables of HAL are provided [here](https://github.com/zharoldr/HAL/releases).

## Setup
- Clone the repo: `git clone git@github.com:zharoldr/HAL.git`
- Enter the repo: `cd HAL`
- Get submodules: `git submodule update --init --recursive`
- Setup the Unity Project: `./setup.sh`

## Unity
HAL is made in Unity 2020.3.21f1. The editor can be downloaded from the [Unity Download Archive](https://unity3d.com/get-unity/download/archive). With the editor installed, the HAL Unity project can be opened as shown: ![Open Project in Unity](/docs/add_project.png)

## Changing the Robot
HAL depends on a robot prefab called `HAL-Robot`, which by default is built from the [`kobuki_hexagons_asus_xtion_pro` Turtlebot](https://github.com/turtlebot/turtlebot/blob/melodic/turtlebot_description/robots/kobuki_hexagons_asus_xtion_pro.urdf.xacro). ![HAL Script](/docs/hal_script.png)

The `HAL-Robot` prefab was built from the Turtlebot URDF using the following steps:

1) Go to `GameObject > 3D Object > URDF Model (import)`: ![Import Step 1](/docs/import_urdf.png)
2) Select the URDF to import and click `Open`: ![Import Step 2](/docs/import_urdf_2.png)
3) Change `Select Axis Type` to `Z Axis` and click `Import URDF`:![Import Step 3](/docs/import_urdf_3.png)
4) Now that the URDF has been imported, go to `Window > URDF-to-Hal`:![Convert Step 1](/docs/convert_urdf.png)
5) Drag-and-drop the converted URDF from the scene Hierarchy into the `Imported URDF Tree` field and click `Convert to Prefab for HAL`:![Convert Step 2](/docs/convert_urdf_2.png)
6) In the scene hierarchy, click on `HAL`. Drag-and-drop the newly created `HAL-Robot` prefab into the `Robot_model` field in the inspector:![Add Robot](/docs/add_robot.png)
7) Delete the imported URDF and `HAL-Robot` from the scene hierarchy. The new `HAL-Robot` prefab was automatically saved to the `Assets/Prefabs` folder.