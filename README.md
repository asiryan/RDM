<p align="center"><img width="40%" src="docs/rdm_logo_main.png" /></p>   

**Range-difference method** implementation using C#.   

# Abstract
One of the most common methods for determining the coordinates of a target in passive systems of positioning is the range-difference method. As a navigation parameter the range-difference method uses the difference of distances from the target to the spaced synchronized receivers determined by mutual time delay of the received signals.  
This implementation uses an algorithm for determining target coordinates by five time-synchronized receivers by solving a linearized system of equations [1].  

<p align="center"><img width="40%" src="docs/satellites.png" /></p>   
<p align="center"><b>Figure 1.</b> Multiposition Satellite System</p>   

# Code 
Download and build all projects from [**sources**](sources) folder.
Run sample console application *RDM_CONSOLE.exe*.

```c#

```

# License
**GNU GPL v3.0**  

# References
[1] **I.V. Grin, R.A. Ershov, O.A. Morozov, V.R. Fidelman** - Evaluation of radio source’s coordinates based on solution of linearized system of equations by range-difference method ([pdf](https://cyberleninka.ru/article/n/otsenka-koordinat-istochnika-radioizlucheniya-na-osnove-resheniya-linearizovannoy-sistemy-uravneniy-raznostno-dalnomernogo-metoda/pdf)).  
[2] **V.B. Burdin, V.A. Tyurin, S.A. Tyurin, V.M. Asiryan** - The estimation of target positioning by means of the range-difference method (***not available***).  
[3] **E.P. Voroshilin, M.V. Mironov, V.A. Gromov** - The estimation of radio source positioning by means of the range-difference method using the multiposition passive satellite system ([pdf](https://cyberleninka.ru/article/n/opredelenie-koordinat-istochnikov-radioizlucheniya-raznostno-dalnomernym-metodom-s-ispolzovaniem-gruppirovki-nizkoorbitalnyh-malyh/pdf)).  
[4] Coordinate system on **Wiki** ([page](https://en.wikipedia.org/wiki/Coordinate_system)).  
