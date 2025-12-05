# UnitySNNShader
A Unity project demonstrating GPU-accelerated spiking neural network shaders.

## The Setup:
1. All SNNs are standard HLSL shader implementations of their respective models.
    - Each shader computes the model across 3 color channels `R,G,B` simulating each color channel as an independent instance of the model
2. Each model's simulation has the following:
    - CustomRenderTexture-based shaders implementing the equations for each of the state variables the model
    - A material referencing each shader, which instantiates the shader
    - A CustomRenderTexture referencing each material allowing the values to be updated via script
    - A Simulation script that manages references to the materials and CustomRenderTextures and makes sure all state variables are updated synchronously
    - A NetworkClock that manages simulation speed
3. Each model is implemented in a separate scene with an input image $I$ and the output voltage $V$ visualized
4. All simulations are integrated using Explicit Euler with a timestep proportional to the simulation speed
    - Note that this can lead to instability in the Hodgkin Huxley model especially, but in all others at low framerates as well
5. All shaders require the use of clamping for stability. This means that infinities are not possible, but that the model may misbehave at unreasonable values (very fun to play with).

# Example Scenes
For all scenes:
- $I$ is computed as a scaled input from the input image
- A slider is available to manipulate simulation speed
- **WARNING**: Certain simulation configurations can lead to very fast flashing images.

## 1. LIF Scene

<img width="971" height="594" alt="image" src="https://github.com/user-attachments/assets/95e5918c-82ef-48b1-90bf-d95036f288fe" />

### LIF Equation: 
$\frac{dV}{dt} = \frac{-(V - V_0)}{RC} + \frac{I}{C}$

if $V > V_{thresh}$, then $V \leftarrow V_0$

### Controls: 
Sliders and Color Pickers to manipulate $V_0$, $R$, and $C$

## 2. Izhikevich Scene

<img width="1691" height="948" alt="image" src="https://github.com/user-attachments/assets/2cf958d4-1c81-4750-9dd9-ece93ddd47bc" />

### Izhikevich Equations:

$\frac{dV}{dt} = 0.04V^2 + 5V + 140 - U + I$

$\frac{dU}{dt} = a(bV - U)$

if $V = 30 mV$, 

then $v \leftarrow c$, $u \leftarrow u+d$

### Controls: 
Sliders and Color Pickers to manipulate $a$, $b$, $c$, and $d$

## 3. Hodgkin Huxley Scene

<img width="1712" height="958" alt="image" src="https://github.com/user-attachments/assets/2041af2c-3030-459c-99b1-25cb58198d1a" />

### Hodgkin Huxley Equations:

$$
C = 1
$$

$$
g_{\text{Na}} = 120,\quad g_{\text{K}} = 36,\quad g_{\text{L}} = 0.3
$$

$$
E_{\text{Na}} = 50,\quad E_{\text{K}} = -77,\quad E_{\text{L}} = -54.387
$$

Membrane equation:

$$
C \frac{dV}{dt} = I(t) - I_{\text{ion}}(V, m, h, n)
$$

Total ionic current:

$$
I_{\text{ion}}(V, m, h, n) = I_{\text{Na}}(V, m, h) + I_{\text{K}}(V, n) + I_{\text{L}}(V)
$$

Sodium, potassium, and leak currents:

$$
I_{\text{Na}}(V, m, h) = g_{\text{Na}} m^3 h \bigl(V - E_{\text{Na}}\bigr)
$$

$$
I_{\text{K}}(V, n) = g_{\text{K}} n^4 \bigl(V - E_{\text{K}}\bigr)
$$

$$
I_{\text{L}}(V) = g_{\text{L}} \bigl(V - E_{\text{L}}\bigr)
$$

Gating variable dynamics:

$$
\frac{dm}{dt} = \alpha_m(V) \bigl(1 - m\bigr) - \beta_m(V)\,m
$$

$$
\frac{dn}{dt} = \alpha_n(V) \bigl(1 - n\bigr) - \beta_n(V)\,n
$$

$$
\frac{dh}{dt} = \alpha_h(V) \bigl(1 - h\bigr) - \beta_h(V)\,h
$$

Rate functions:

$$
\alpha_m(V) = 0.1 \frac{V + 40}{1 - \exp\left(-\frac{V + 40}{10}\right)}
$$

$$
\beta_m(V) = 4 \exp\left(-\frac{V + 65}{18}\right)
$$

$$
\alpha_n(V) = 0.01 \frac{V + 55}{1 - \exp\left(-\frac{V + 55}{10}\right)}
$$

$$
\beta_n(V) = 0.125 \exp\left(-\frac{V + 65}{80}\right)
$$

$$
\alpha_h(V) = 0.07 \exp\left(-\frac{V + 65}{20}\right)
$$

$$
\beta_h(V) = \frac{1}{1 + \exp\left(-\frac{V + 35}{10}\right)}
$$

### Controls:

Sliders and Color Pickers to manipulate 
- $g_{Na}$, $E_{Na}$
- $g_K$, $E_K$
- $g_L$, $E_L$
- $C$



