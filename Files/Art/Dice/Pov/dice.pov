//  Persistence of Vision Raytracer V3.1
//  World definition file.
//
//  Contains 1 lights, 2 materials and 23 primitives.
//
//  This file was generated for POV-Ray V3.1 by
//  Moray V3.3a For Windows Copyright (c) 1993-2001 Lutz + Kretzschmar
//

//  Date : 07/12/2002    (12.07.2002)
//

/*
  The text between these two comments is in MorayPOV.INC and is
  automatically included in all POV files that Moray exports.
*/

default {
  texture {
    pigment { rgb <1,0,0> }
  }
}

/* // Scene Comment

This scene was created with Moray For Windows.

*/ // End Scene Comment

global_settings {
  adc_bailout 0.003922
  ambient_light <1.0,1.0,1.0>
  assumed_gamma 1.9
  hf_gray_16 off
  irid_wavelength <0.247059,0.176471,0.137255>
  max_intersections 64
  max_trace_level 10
  number_of_waves 10
}

background { color <0.000,0.000,0.000> }

camera {  //  Camera StdCam
  location  <     -3.000,       3.000,       4.000>
  sky       <    0.00000,     0.00000,     1.00000> // Use right handed-system 
  up        <        0.0,         0.0,         1.0> // Where Z is up
  right     <    1.00000,         0.0,         0.0> // Right Vector is adjusted to compensate for spherical (Moray) vs. planar (POV-Ray) aspect ratio
  angle         39.60000    // Vertical      39.600
  look_at   <      0.000,       0.000,       0.000>
}

//
// *******  L I G H T S *******
//

light_source {   // Light001
  <0.0, 0.0, 0.0>
  color rgb <1.000, 1.000, 1.000>
  translate  <-18.754392, -0.401769, 61.307308>
}


//
// ********  MATERIALS  *******
//

#include "dice.inc"


//
// ********  REFERENCED OBJECTS  *******
//



//
// ********  OBJECTS  *******
//

               
#declare Body =                 
  box { <-1, -1, -1>, <1, 1, 1> 
  
 
   texture {
      pigment {  color rgbf <0.75, 0.0004, 0.0, 0.4> }
      finish {
         //phong 0.2
         //phong_size 10
         ambient 0.2
         diffuse 0.8         
      }
   }
}                

#declare PIPX=0.475;      
#declare PIPZ=0.801;      

#declare Twos = union {
  cone { // Cone020
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate <-180.0, -270.0, -180.0>
    translate  <0.0, -PIPX, PIPX>
  }
  cone { // Cone021
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate <-180.0, 90.0, -180.0>
    translate  <0.0, PIPX, -PIPX>
  }
  translate  PIPZ*x
}
#declare Fives = union {
  cone { // Cone018
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate <-180.0, 90.0, 0.0>
    translate  <0.0, -PIPX, PIPX>
  }
  cone { // Cone019
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate <-180.0, 90.0, 0.0>
    translate  <0.0, PIPX, -PIPX>
  }
  cone { // Cone017
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate <-180.0, 90.0, 0.0>
    translate  <0.0, -PIPX, -PIPX>
  }
  cone { // Cone016
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate <-180.0, 90.0, 0.0>
    translate  <0.0, PIPX, PIPX>
  }
  cone { // Cone008
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate <-180.0, 90.0, 0.0>
  }
  translate  -PIPZ*x
}
#declare Sixes = union {
  cone { // Cone007
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate 180.0*x
    translate  PIPX*x
  }
  cone { // Cone006
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate 180.0*x
    translate  <PIPX, -PIPX, 0.0>
  }
  cone { // Cone005
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate 180.0*x
    translate  <PIPX, PIPX, 0.0>
  }
  cone { // Cone004
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate 180.0*x
    translate  -PIPX*x
  }
  cone { // Cone002
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate 180.0*x
    translate  <-PIPX, PIPX, 0.0>
  }
  cone { // Cone003
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate 180.0*x
    translate  <-PIPX, -PIPX, 0.0>
  }
  translate  -PIPZ*z
}
#declare Threes = union {
  cone { // Cone011
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate -90.0*x
    translate  <PIPX, 0.0, -PIPX>
  }
  cone { // Cone010
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate -90.0*x
  }
  cone { // Cone009
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate -90.0*x
    translate  <-PIPX, 0.0, PIPX>
  }
  translate  PIPZ*y
}
#declare Fours = union {
  cone { // Cone015
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate 90.0*x
    translate  <-PIPX, 0.0, -PIPX>
  }
  cone { // Cone014
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate 90.0*x
    translate  <-PIPX, 0.0, PIPX>
  }
  cone { // Cone013
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate 90.0*x
    translate  <PIPX, 0.0, PIPX>
  }
  cone { // Cone012
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    rotate 90.0*x
    translate  <PIPX, 0.0, -PIPX>
  }
  translate  -PIPZ*y
}
union { // Die
  object { Twos }
  object { Fives }
  object { Sixes }
  object { Threes }
  object { Fours }
  cone { // PipOne
    <0,0,0>, 0.0, <0,0,1>, 1.0
    material {
      Pips
    }
    scale 0.2
    translate  PIPZ*z
  }
  
  object {Body}  
  rotate <0,-clock*10-90,-clock*10>

}


