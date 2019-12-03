import torch
import torch.nn as nn
import torch.nn.functional as F


class Net(nn.Module):

    def __init__(self):
        super().__init__()
        # for a line of skeleton data, there are 75 numbers and 7 categories
        self.fc_layer1 = nn.Linear(75, 100)
        self.fc_layer2 = nn.Linear(100, 100)
        # self.fc_layer3 = nn.Linear(48, 48)
        self.fc_layer4 = nn.Linear(100, 7)

    def forward(self, x):
        x = F.leaky_relu(self.fc_layer1(x))
        x = F.leaky_relu(self.fc_layer2(x))
        # x = F.leaky_relu(self.fc_layer3(x))
        x = self.fc_layer4(x)
        return x
