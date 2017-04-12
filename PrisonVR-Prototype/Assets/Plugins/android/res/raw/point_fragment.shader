precision mediump float;
varying vec4 v_color;    // The input texture.
void main() {
    gl_FragColor = v_color; // float[4](1.0, 0.0, 0.0, 1.0);
}
