attribute vec4 a_Position;
uniform mat4 u_MVPP;
uniform vec4 a_color;    // The input texture.
varying vec4 v_color;    // The input texture.
void main() {
   gl_Position = u_MVPP * a_Position;
   gl_PointSize = 13.0;
   v_color = a_color;
}