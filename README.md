Unleashing the Power of Unity 3D and MRTK to Fly a Drone in Mixed Reality!


**Build Instructions**

When building from UWP, you will get similar errors like  **** does not exist in the namespace 'Microsoft.Graphics' (are you missing an assembly reference?)
Please replace Graphics.*** to UnityEngine.Graphics.****. These changes cached only on local library folder.

eg> Graphics.ExecuteCommandBuffer(cmd); to UnityEngine.Graphics.ExecuteCommandBuffer(cmd);

From Unity, build as UWP project. Once the build is successfull, open the project with Visual studo.
You need to add the https://www.nuget.org/packages/Win2D.uwp/1.26.0 using nuget packagamanager in the visual studio. 

Note *we already have this a package inside unity, somehow it is not exporting. If fixed you dont need to add again in visual studio.

After adding the package, Choose build Target as ARM64 - Release mode, build and run it on HL2

Note * video rendering only supported on UWP platfomrs
