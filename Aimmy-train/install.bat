python -m venv venv
call venv/scripts/activate
.\venv\Scripts\python.exe -m pip install --upgrade pip
pip install torch torchvision --index-url https://download.pytorch.org/whl/cu118
pip install ultralytics onnx tensorboard
cmd
