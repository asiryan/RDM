<p align="center"><img width="40%" src="docs/rdm_logo_main.png" /></p>  

**Range-difference method** implementation using C#.   

# Abstract
One of the most common methods for determining the coordinates of a target in passive systems of positioning is the range-difference method (**RDM**). As a navigation parameter the RDM uses the difference of distances from the target to the spaced synchronized receivers determined by mutual time delay of the received signals.  
The RDM for determining the target coordinates can be implemented for 5 (or more) time-synchronized receivers by solving a **linearized system of equations** [1] or for 2, 3 (1D, 2D coordinates) and 4 (3D coordinates) receivers by solving a **nonlinear system of equations** [2, 3]. This implementation uses both algorithms and can finds the coordinates of the target for **2 or more receivers**.  

<p align="center"><img width="50%" src="docs/satellites.png" /></p>  
<p align="center"><b>Figure 1.</b> Multiposition Satellite System</p>  

# Installation
Download from [**release**](release) folder and add **RDM.dll** to your project.  
```c#
using RDM;
```

# Code
Download and build all projects from [**sources**](sources) folder.  

## Console application
It simulates the situation of receiving data from a map about the location of the target in **geodetic coordinates** [4]. The coordinates are transformed from geodetic to Cartesian, and receivers are randomly placed in accordance with the scaling vector, and the time delays of the signal are calculated. The **RDM** is applied, and target coordinates and quality metrics are calculated. Finally, the the target coordinates are transformed back to geodetic coordinates.  
  
Run ***RDM_CONSOLE.exe***  

```
Target (Geodetic): 80, 20, 100
Target (Cartesian): 1044169.258983, 380046.529794, 6259644.553094
Scaling (Cartesian): 700, 800, 500
Sigma: 0.5
Receivers count: 4

Receiver: 1043610.564058, 379904.752545, 6259459.748037
Receiver: 1043632.702017, 380065.032217, 6259505.277246
Receiver: 1043888.431175, 380815.295547, 6259288.786147
Receiver: 1044714.153376, 379445.989604, 6259157.173841
Time delays: 2E-06, 2E-06, 3E-06, 3E-06
RDM (Cartesian): 1044169.25887, 380046.529997, 6259644.549067

Accuracy: 0.999999999375374
Similarity: 1
Loss: 0.00434318458428606

RDM (Geodetic): 80, 20, 99.996028
```

## Windows.Forms application
It simulates two models: random placement of receivers at a fixed location of the target and random placement of the target at fixed locations of the receivers. The coordinates of objects are visualized using [**ZedGraph**](https://sourceforge.net/projects/zedgraph/) and the **RDM** is applied.  

Run ***RDM_VISUAL.exe***, apply settings and press "*Generate*" button.  
Double click on the graph and save the image.  

<p align="center"><img width="60%" src="docs/graph.png" /></p>  
<p align="center"><b>Figure 2.</b> Saved graph image</p>  

# License
**MIT License**  

# References
[1] **I.V. Grin, R.A. Ershov, O.A. Morozov, V.R. Fidelman** - Evaluation of radio source’s coordinates based on solution of linearized system of equations by range-difference method ([***pdf***](https://cyberleninka.ru/article/n/otsenka-koordinat-istochnika-radioizlucheniya-na-osnove-resheniya-linearizovannoy-sistemy-uravneniy-raznostno-dalnomernogo-metoda/pdf)).  
[2] **V.B. Burdin, V.A. Tyurin, S.A. Tyurin, V.M. Asiryan** - The estimation of target positioning by means of the range-difference method ([***pdf***](https://github.com/asiryan/asiryan/blob/main/Publications/%D0%92.%D0%91.%20%D0%91%D1%83%D1%80%D0%B4%D0%B8%D0%BD%2C%20%D0%92.%D0%90.%20%D0%A2%D1%8E%D1%80%D0%B8%D0%BD%2C%20%D0%A1.%D0%90.%20%D0%A2%D1%8E%D1%80%D0%B8%D0%BD%2C%20%D0%92.%D0%9C.%20%D0%90%D1%81%D0%B8%D1%80%D1%8F%D0%BD%20-%20%D0%9E%D0%BF%D1%80%D0%B5%D0%B4%D0%B5%D0%BB%D0%B5%D0%BD%D0%B8%D0%B5%20%D0%BA%D0%BE%D0%BE%D1%80%D0%B4%D0%B8%D0%BD%D0%B0%D1%82%20%D1%86%D0%B5%D0%BB%D0%B8%20%D1%80%D0%B0%D0%B7%D0%BD%D0%BE%D1%81%D1%82%D0%BD%D0%BE-%D0%B4%D0%B0%D0%BB%D1%8C%D0%BD%D0%BE%D0%BC%D0%B5%D1%80%D0%BD%D1%8B%D0%BC%20%D0%BC%D0%B5%D1%82%D0%BE%D0%B4%D0%BE%D0%BC.pdf)).  
[3] **E.P. Voroshilin, M.V. Mironov, V.A. Gromov** - The estimation of radio source positioning by means of the range-difference method using the multiposition passive satellite system ([***pdf***](https://cyberleninka.ru/article/n/opredelenie-koordinat-istochnikov-radioizlucheniya-raznostno-dalnomernym-metodom-s-ispolzovaniem-gruppirovki-nizkoorbitalnyh-malyh/pdf)).  
[4] Coordinate system on **Wiki** ([***page***](https://en.wikipedia.org/wiki/Coordinate_system)).  
