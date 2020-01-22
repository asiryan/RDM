<p align="center"><img width="40%" src="docs/rdm_logo_main.png" /></p>   

**Range-difference method** implementation using C#.   

# Abstract
One of the most common methods for determining the coordinates of a target in passive systems of positioning is the range-difference method. As a navigation parameter the range-difference method uses the difference of distances from the target to the spaced synchronized receivers determined by mutual time delay of the received signals.  
This implementation uses an algorithm for determining target coordinates by five time-synchronized receivers by solving a linearized system of equations [1].  

<p align="center"><img width="40%" src="docs/satellites.png" /></p>   
<p align="center"><b>Figure 1.</b> Multiposition Satellite System</p>   

# Code 
Download and build all projects from [**sources**](sources) folder.  

## Console application
Run ***RDM_CONSOLE.exe***  

```
Target (Geodetic): 80, 20, 100
Target (Cartesian): 1044169.258983, 380046.529794, 6259644.553094
Scaling (Cartesian): 1000, 1000, 1000
Sigma: 0.5

Receiver: 1043344.154636, 379221.425447, 6258819.448747
Receiver: 1043464.7512, 380751.037577, 6258940.045311
Receiver: 1044775.1401, 379440.648677, 6259038.671976
Receiver: 1045569.491596, 381446.762407, 6258244.320481
Receiver: 1046796.760423, 381360.280514, 6258330.802374
Time: 5E-06, 4E-06, 4E-06, 8E-06, 1.1E-05
RDM (Cartesian): 1044169.258986, 380046.529796, 6259644.553079

Accuracy: 0.999999999997838
Loss: 8.61359438577255E-06

RDM (Geodetic): 80, 20, 99.999986
```

## Windows.Forms application
Run ***RDM_VISUAL.exe***, apply settings and press **Generate** button.  
Double click on the graph pane and save the image.  

<p align="center"><img width="60%" src="docs/graph.png" /></p>
<p align="center"><b>Figure 4.</b> Saved graph pane</p>   

# License
**GNU GPL v3.0**  

# References
[1] **I.V. Grin, R.A. Ershov, O.A. Morozov, V.R. Fidelman** - Evaluation of radio source’s coordinates based on solution of linearized system of equations by range-difference method ([***pdf***](https://cyberleninka.ru/article/n/otsenka-koordinat-istochnika-radioizlucheniya-na-osnove-resheniya-linearizovannoy-sistemy-uravneniy-raznostno-dalnomernogo-metoda/pdf)).  
[2] **V.B. Burdin, V.A. Tyurin, S.A. Tyurin, V.M. Asiryan** - The estimation of target positioning by means of the range-difference method (***not available***).  
[3] **E.P. Voroshilin, M.V. Mironov, V.A. Gromov** - The estimation of radio source positioning by means of the range-difference method using the multiposition passive satellite system ([***pdf***](https://cyberleninka.ru/article/n/opredelenie-koordinat-istochnikov-radioizlucheniya-raznostno-dalnomernym-metodom-s-ispolzovaniem-gruppirovki-nizkoorbitalnyh-malyh/pdf)).  
[4] Coordinate system on **Wiki** ([***page***](https://en.wikipedia.org/wiki/Coordinate_system)).  
