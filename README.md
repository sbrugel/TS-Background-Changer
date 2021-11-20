# TS-Background-Changer
Software that allows the user to change the menu background of Train Simulator 20XX.

## About
This is an easy-to-use utility that allows the user to upload an image of their choosing, that can then be used to change the menu background of Train Simulator 2022.

## Notable Features
- RailWorks directory automatically detected through registry where applicable
- Images that are not the exact size TS uses are automatically scaled before background modification

## Program Notes
The image used for the Train Simulator menu background is a JPG of 1920x1080 resolution. By default, this program's image selected should only allow to choose JPG files. You are able to choose pictures of any size; however, it is strongly recommended to choose an image that is 1920x1080, or one that is at least of a 16:9 aspect ratio. Otherwise, the program will stretch the image to exactly fit this dimension. 

The main button's background color will change depending on the image selected.
- If it is GREEN, the image is 1920x1080 and will not change size upon being copied into Train Simulator.
- If it is YELLOW, the image is not 1920x1080 but is of a 16:9 aspect ratio. While it will not be stretched out of proportion, its size will change upon being copied into Train Simulator and noticeable quality loss may occur.
- If it is RED, the image is not 1920x1080 and is not of a 16:9 aspect ratio. This image will be stretched to be 1920x1080 upon being copied into Train Simulator, and it may look out of proportion.

