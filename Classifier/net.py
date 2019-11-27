import torch.nn as nn
import torch.nn.functional as F


class Net(nn.Module):

   def __init__(self):
       super().__init__()
        self.fc_layer1 = nn.Linear(28*28, 64)
        self.fc_layer2 = nn.Linear(64, 64)
        self.fc_layer3 = nn.Linear(64, 64)
        self.fc_layer4 = nn.Linear(64, 10)

    def forward(self, x):
        x = F.leaky_relu(self.fc_layer1(x))
        x = F.leaky_relu(self.fc_layer2(x))
        x = F.leaky_relu(self.fc_layer3(x))
        x = self.fc_layer4(x)
        return F.log_softmax(x, dim=1)
