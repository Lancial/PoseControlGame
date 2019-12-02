import torch
import torchvision
from torchvision import transforms, datasets
import torch.nn as nn
import torch.nn.functional as F
import torch.optim as optim
from net import Net
from KinectDataSet import KinectDataSet


# train = datasets.MNIST('', train=True, download=True,
#                        transform=transforms.Compose([transforms.ToTensor()]))

# test = datasets.MNIST('', train=False, download=True,
#                       transform=transforms.Compose([transforms.ToTensor()]))

# trainset = torch.utils.data.DataLoader(train, batch_size=10, shuffle=True)
# testset = torch.utils.data.DataLoader(test, batch_size=10, shuffle=False)

# load training and testing data set

train = KinectDataSet(csv_file='./../Kinect455/Kinect4Win/KinectForWindow/Assets/StreamingAssets/bodydata.csv',
                      root_dir='./../Kinect455/Kinect4Win/KinectForWindow/Assets/StreamingAssets/')
trainset = torch.utils.data.DataLoader(train, batch_size=10, shuffle=True)

net = Net()

loss_function = nn.CrossEntropyLoss()
optimizer = optim.Adam(net.parameters(), lr=10e-4) # adam or SGD? what are they

for epoch in range(3):
    for data in trainset:
        X, y = data['skeleton_data'], data['poss_label']
        optimizer.zero_grad()  # don't really understand this part, before is net.zero_grad()
        output = net(X.view(-1, 75))  # flat the data
        loss = loss_function(output, y) # use CrossEntropyLoss
        loss.backward()
        optimizer.step()
    print(loss)

correct = 0
total = 0

with torch.no_grad():
    for data in trainset:
        X, y = data
        output = net(X.view(-1, 784))
        for idx, i in enumerate(output):
            if torch.argmax(i) == y[idx]:
                correct += 1
            total += 1

print("Accuracy: ", round(correct/total, 3))
