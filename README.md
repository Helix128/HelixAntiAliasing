# HAA
 Fast Approximate anti aliasing solution for Unity. Mainly created as an experiment.
 
# Usage
  Just add the HAA component to a camera and it should already be working in the game view.
  
# Settings
  
  Strength: Strength of the softening applied to the edges.
  
  Threshold: Threshold of edge detection. 
  
  Sharpen: Optional sharpening to the edges.
  
# Compatibility
  
  This anti aliasing uses Compute Shaders (yeah, blame me for that) so it probably wont work on mobiles. I have only tested it on my PC with DirectX 11.

# How does it work?

 Step 1: Gather the borders of the image by subtracting 4 samples of it but displaced in the x and y axis based on the threshold value.
 
 Step 2: For each sample that is in a border, sample 4 more samples of neighbor pixels, the selected pixels are at a certain distance depending on the strength.
 
 Step 3: Just multiply the border value by the sharpen value and add it to the result
