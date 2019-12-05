import torch
import torchvision
from torchvision import transforms, datasets
import torch.nn as nn
import torch.nn.functional as F
import torch.optim as optim
from pathlib import Path
from net import Net
from KinectDataSet import KinectDataSet
from os import path

# load training and testing data set
csv_directory = Path(
    "../Kinect455/Kinect4Win/KinectForWindow/Assets/StreamingAssets/")
if not csv_directory.exists():
    csv_directory = Path(
        "Kinect455/Kinect4Win/KinectForWindow/Assets/StreamingAssets/")
train_csv_path = csv_directory / "bodydata_train1.csv"
test_csv_path = csv_directory / "bodydata_test1.csv"

train = KinectDataSet(csv_file=train_csv_path,
                      root_dir=csv_directory)
test = KinectDataSet(csv_file=test_csv_path,
                     root_dir=csv_directory)

trainset = torch.utils.data.DataLoader(train, batch_size=9, shuffle=True)
testset = torch.utils.data.DataLoader(test, batch_size=9, shuffle=True)

net = Net()

loss_function = nn.CrossEntropyLoss()
# adam or SGD? what are they
optimizer = optim.SGD(net.parameters(), lr=10e-4)

for epoch in range(100):
    for data in trainset:
        X, y = data
        optimizer.zero_grad()  # don't really understand this part, before is net.zero_grad()
        # output = net(X.view(-1, 75))  # flat the data
        output = net(X)
        loss = loss_function(output, y.long())  # use CrossEntropyLoss
        loss.backward()
        optimizer.step()
    print(loss)

correct = 0
total = 0

with torch.no_grad():
    for data in trainset:
        X, y = data
        # output = net(X.view(-1, 784))
        output = net(X)
        for idx, i in enumerate(output):
            if torch.argmax(i) == y[idx]:
                correct += 1
            total += 1

print("training accuracy: ", round(correct/total, 3))

correct = 0
total = 0
with torch.no_grad():
    for data in testset:
        X, y = data
        # output = net(X.view(-1, 784))
        output = net(X)
        for idx, i in enumerate(output):
            if torch.argmax(i) == y[idx]:
                correct += 1
            total += 1

print("testing accuracy: ", round(correct/total, 3))
#save model to disk
model_path = path.join(path.dirname(path.realpath(__file__)), 'model.pth')
torch.save(net.state_dict(), model_path)
