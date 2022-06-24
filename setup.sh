#!/bin/bash


rm -rf Assets/URDF*
rm -rf Deps/build/
rm -rf Deps/devel/
rm -rf Deps/src/ar_track_alvar/ar_track_alvar/ Turtlebot/src/ar_track_alvar.rosinstall

cd Deps && catkin_make

cd src/turtlebot/turtlebot_description/robots

rosrun xacro xacro -o robot.urdf kobuki_hexagons_asus_xtion_pro.urdf.xacro

mkdir ../../../../../Assets/URDF

mv robot.urdf ../../../../../Assets/URDF/robot.urdf

cp -r ../../../kobuki/kobuki_description ../../../../../Assets/URDF/kobuki_description
cp -r ../../turtlebot_description ../../../../../Assets/URDF/turtlebot_description

cd ../../../../../Assets/URDF

sed '/.*collision.*/d' robot.urdf > t.urdf
cat t.urdf > robot.urdf

sed '/<collision/,/\/collision>/{/.*/d;/.*/d}' robot.urdf > t.urdf
cat t.urdf > robot.urdf

sed '/<inertial/,/\/inertial>/{/.*/d;/.*/d}' robot.urdf > t.urdf
cat t.urdf > robot.urdf

sed '/<plugin/,/\/plugin>/{/.*/d;/.*/d}' robot.urdf > t.urdf
cat t.urdf > robot.urdf

sed '/<sensor/,/\/sensor>/{/.*/d;/.*/d}' robot.urdf > t.urdf
cat t.urdf > robot.urdf

rm -f t.urdf
