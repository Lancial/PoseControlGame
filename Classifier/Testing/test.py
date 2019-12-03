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

train = SimpleDataSet(csv_file=csv_path, root_dir=csv_directory,
                      transform=transformations)
trainset = torch.utils.data.DataLoader(train, batch_size=2, shuffle=False)

test = SimpleDataSet(
    csv_file=csv_path, root_dir=csv_directory, transform=transformations)
testset = torch.utils.data.DataLoader(test, batch_size=2, shuffle=False)

net = SimpleNet()

loss_function = nn.MSELoss()
# adam or SGD? what are they
optimizer = optim.SGD(net.parameters(), lr=10e-4, momentum=0.9)

# for epoch in range(100):
# for data in trainset:
#     X, y = data
#     optimizer.zero_grad()  # don't really understand this part, before is net.zero_grad()
#     # print(X)
#     # print(y)
#     output = net(X)
#     # output = net(X.view(-1, 9))  # flat the data
#     loss = loss_function(output, y.float())  # use CrossEntropyLoss
#     loss.backward()
#     optimizer.step()
# print(loss)

for epoch in range(5):
    for i, (images, labels) in enumerate(trainset):
        images = Variable(images)
        labels = Variable(labels)
        # Clear gradients
        optimizer.zero_grad()
        # Forward pass
        outputs = net(images)
        # Calculate loss
        loss = loss_function(outputs, labels.float())
        # Backward pass
        loss.backward()
        # Update weights
        optimizer.step()
    print(loss)

correct = 0
total = 0

with torch.no_grad():
    for data in testset:
        X, y = data
        output = net(X)
        # for idx, i in enumerate(output):
        #     if torch.argmax(i) == y[idx]:
        #         print(torch.argmax(i), y[idx])
        #         correct += 1
        #     total += 1
        _, predicted = torch.max(outputs.data, 1)
        total += labels.size(0)
        correct += (predicted == labels).sum().item()
        print(predicted, labels)

print("Accuracy: ", round(correct/total, 3))
