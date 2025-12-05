# UnitySNNShader
A Unity project demonstrating GPU-accelerated spiking neural network shaders.
**The Setup**:
1. All SNNs are standard HLSL shader implementations of their respective models.
- Each shader computes the model across 3 color channels `R,G,B` simulating each color channel as an independent instance of the model
2. Each model's simulation has the following:
  - CustomRenderTexture-based shaders implementing the equations for each of the state variables the model
  - A material referencing each shader, which instantiates the shader
  - A CustomRenderTexture referencing each material allowing the values to be updated via script
  - A Simulation script that manages references to the materials and CustomRenderTextures and makes sure all state variables are updated synchronously
  - A NetworkClock that manages simulation speed
3. Each model is implemented in a separate scene with an input image $I$ and the output voltage $V$ visualized

## Example Scenes
For all scenes:
- $I$ is computed as a scaled input from the input image
- A slider is available to manipulate simulation speed
### 1. LIF Scene
LIF Equation: $\frac{dV}{dt} = \frac{-(V - V_0)}{RC} + \frac{I}{C}$

**Controls**: Sliders and Color Pickers to manipulate $V_0$, $R$, and $C$

<img width="971" height="594" alt="image" src="https://github.com/user-attachments/assets/95e5918c-82ef-48b1-90bf-d95036f288fe" />

### 2. Izhikevich Scene
Izhikevich Equations:

$\frac{dV}{dt} = 0.04V^2 + 5V + 140 - U + I$

$\frac{dU}{dt} = a(bV - U)$

if $V = 30 mV$, 

then $v \leftarrow c$, $u \leftarrow u+d$

**Controls**: Sliders and Color Pickers to manipulate $a$, $b$, $c$, and $d$


<img width="1691" height="948" alt="image" src="https://github.com/user-attachments/assets/2cf958d4-1c81-4750-9dd9-ece93ddd47bc" />


