# kineVIVE

First version of kine VIVE.

## FEATURES

1. Calibration of up to 5 trackers (key "l")
2. Automated asignment of tracker roles (UNTESTED!!! serialNumbers.cs+Roles.cs+config.json)
3. UDP Server implemented (UDPReceive.cs bound to a server object, lastReceivedUDPPacket is the var that stores messages)
4. Performance monitor (sampling.cs is way of checking fps)

## TO DO

1. Implement SG_Handfeedback logic to project and clean SenseGlove scripts
2. Allow for 2 modes of operation (kineVIVE+TRS matrix)
