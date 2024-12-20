call venv/scripts/activate
yolo export model=.\runs\detect\Train\weights\best.pt format=onnx
cmd