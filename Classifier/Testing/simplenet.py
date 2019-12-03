import torch
import torch.nn as nn
import torch.nn.functional as F


class SimpleNet(nn.Module):

    def __init__(self):
        super(SimpleNet, self).__init__()
        # for a line of skeleton data, there are 75 numbers and 7 categories
        self.fc_layer1 = nn.Linear(9, 6)
        self.fc_layer2 = nn.Linear(6, 6)
        self.fc_layer3 = nn.Linear(6, 2)

    def forward(self, x):
        x = F.sigmoid(self.fc_layer1(x))
        x = F.sigmoid(self.fc_layer2(x))
        x = self.fc_layer3(x)
        x = x.view(x.size(0), -1)
        return x
