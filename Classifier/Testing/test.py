import torch
import torchvision
from torchvision import transforms, datasets
import torch.nn as nn
import torch.nn.functional as F
import torch.optim as optim
from os import path
from SimpleDataSet import SimpleDataSet
from simplenet import SimpleNet
from torch.autograd import Variable

transformations = transforms.Compose([transforms.ToTensor()])
csv_directory = path.dirname(path.realpath(__file__))
csv_path = path.join(csv_directory, 'skeleton_experiment.csv')

# train = SimpleDataSet(csv_file=csv_path, root_dir=csv_path,
#                       transform=transforms.Compose([transforms.ToTensor()]))
train = SimpleDataSet(csv_file=csv_path, root_dir=csv_path,
                      transform=transformations)
trainset = torch.utils.data.DataLoader(train, batch_size=3, shuffle=True)

net = SimpleNet()

loss_function = nn.MSELoss()
# adam or SGD? what are they
optimizer = optim.Adam(net.parameters(), lr=10e-4)

for epoch in range(3):
    for data in trainset:
        X, y = data
        optimizer.zero_grad()  # don't really understand this part, before is net.zero_grad()
        print(X)
        print(y)
        output = net(X.view(-1, 9))  # flat the data
        loss = loss_function(output, y)  # use CrossEntropyLoss
        loss.backward()
        optimizer.step()
    print(loss)

# for i, (images, labels) in enumerate(trainset):
#     images = Variable(images)
#     labels = Variable(labels)
#     # Clear gradients
#     optimizer.zero_grad()
#     # Forward pass
#     outputs = net(images)
#     # Calculate loss
#     loss = loss_function(outputs, labels)
#     # Backward pass
#     loss.backward()
#     # Update weights
#     optimizer.step()
#     break

correct = 0
total = 0

with torch.no_grad():
    for data in trainset:
        X, y = data
        output = net(X.view(-1, 9))
        for idx, i in enumerate(output):
            if torch.argmax(i) == y[idx]:
                correct += 1
            total += 1

print("Accuracy: ", round(correct/total, 3))
