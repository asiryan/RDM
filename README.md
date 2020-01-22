<p align="center"><img width="40%" src="docs/rdm_logo_main.png" /></p>   

**Range-difference method** implementation using C#.   

# Abstract
One of the most common methods for determining the coordinates of a target in passive systems of positioning is the range-difference method. As a navigation parameter the range-difference method uses the difference of distances from the target to the spaced synchronized receivers determined by mutual time delay of the received signals.  
This implementation uses an algorithm for determining target coordinates by five time-synchronized receivers by solving a linearized system of equations [1].  

<p align="center"><img width="40%" src="docs/satellites.png" /></p>   
<p align="center"><b>Figure 1.</b> Multiposition Satellite System</p>   

# Code 
Download and build all projects from [**sources**](sources) folder.  
Run sample console application ***RDM_CONSOLE.exe***.  

```console
Target (Geodetic): 80, 20, 100

Target (Cartesian): 1044169.258983, 380046.529794, 6259644.553094

Scaling (Cartesian): 1000, 1000, 1000

Sigma: 0.5

Receiver: 1043216.845995, 379094.116806, 6258692.140105

Receiver: 1043478.763122, 380737.025655, 6258954.057232

Receiver: 1045644.98905, 378570.799727, 6258168.823027

Receiver: 1045494.262538, 381371.533349, 6258319.549539

Receiver: 1045500.40044, 380712.100522, 6258978.982365

Time: 6E-06, 4E-06, 9E-06, 8E-06, 5E-06

RDM (Cartesian): 1044169.258977, 380046.529796, 6259644.553118

Accuracy: 0.999999999996426

Loss: 1.42769687086522E-05

RDM (Geodetic): 80, 20, 100.000023
```

# License
**GNU GPL v3.0**  

# References
[1] **I.V. Grin, R.A. Ershov, O.A. Morozov, V.R. Fidelman** - Evaluation of radio source’s coordinates based on solution of linearized system of equations by range-difference method ([***pdf***](https://cyberleninka.ru/article/n/otsenka-koordinat-istochnika-radioizlucheniya-na-osnove-resheniya-linearizovannoy-sistemy-uravneniy-raznostno-dalnomernogo-metoda/pdf)).  
[2] **V.B. Burdin, V.A. Tyurin, S.A. Tyurin, V.M. Asiryan** - The estimation of target positioning by means of the range-difference method (***not available***).  
[3] **E.P. Voroshilin, M.V. Mironov, V.A. Gromov** - The estimation of radio source positioning by means of the range-difference method using the multiposition passive satellite system ([***pdf***](https://cyberleninka.ru/article/n/opredelenie-koordinat-istochnikov-radioizlucheniya-raznostno-dalnomernym-metodom-s-ispolzovaniem-gruppirovki-nizkoorbitalnyh-malyh/pdf)).  
[4] Coordinate system on **Wiki** ([***page***](https://en.wikipedia.org/wiki/Coordinate_system)).  
