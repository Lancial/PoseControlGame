import torch
import onnx
from onnx_tf.backend import prepare
import tensorflow as tf
from os import path
from net import Net
import numpy as np

dir = path.dirname(path.realpath(__file__))
pytorch_path = path.join(dir, './model.pth')

model_pytorch = Net()
model_pytorch.load_state_dict(torch.load(pytorch_path))

# Single pass of dummy variable required
dummy_input = torch.from_numpy(torch.zeros(26)).long()
dummy_output = model_pytorch(dummy_input)
print(dummy_output)

# Export to ONNX format
torch.onnx.export(model_pytorch, 
dummy_input, path.join(dir, './model_onnx.onnx'))

model_onnx = onnx.load(path.join(dir, './model_onnx.onnx'))

tf_rep = prepare(model_onnx)
print(tf_rep.tensor_dict)

# Export model as .pb file
tf_rep.export_graph(path.join(dir, './model_tf.pb'))