<p align="center"><img width="40%" src="docs/rdm_logo_main.png" /></p>   

**Range-difference method** implementation using C#.   

# Abstract
One of the most common methods for determining the coordinates of a target in passive systems of positioning is the range-difference method. As a navigation parameter the range-difference method uses the difference of distances from the target to the spaced synchronized receivers determined by mutual time delay of the received signals.  
This implementation uses an algorithm for determining target coordinates by five time-synchronized receivers by solving a linearized system of equations [1].  

<p align="center"><img width="40%" src="docs/satellites.png" /></p>   
<p align="center"><b>Figure 1.</b> Multiposition Satellite System</p>   


# Code

# Visualization
<p align="center"><img width="40%" src="docs/rdm_visual.png" /></p>   
<p align="center"><b>Figure 2.</b> RDM_VISUAL</p>   


# References
[1] I.V. Grin, R.A. Ershov, O.A. Morozov, V.R. Fidelman - EVALUATION OF RADIO SOURCE’S COORDINATES BASED ON SOLUTION OF LINEARIZED SYSTEM
OF EQUATIONS BY RANGE-DIFFERENCE METHOD
