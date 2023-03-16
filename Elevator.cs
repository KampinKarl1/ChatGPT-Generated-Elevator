public class Elevator {
    private int currentFloor;
    private int destinationFloor;
    private bool isMoving;
    private int topSpeed;
    private int acceleration;
    private int currentSpeed;
    private double position; // Current position of the elevator in meters
    private double[] floorPositions; // Array of floor positions in meters
    private double closeEnough; // The distance within which the elevator is considered to have reached its target floor
    
    public Elevator(int topSpeed, int acceleration, double[] floorPositions, double closeEnough) {
        currentFloor = 1; // Starting floor
        destinationFloor = 1; // Initialize to starting floor
        isMoving = false; // Not moving initially
        this.topSpeed = topSpeed;
        this.acceleration = acceleration;
        currentSpeed = 0;
        position = floorPositions[0]; // Starting position
        this.floorPositions = floorPositions;
        this.closeEnough = closeEnough;
    }
    
    public void Move(int floor) {
        if (IsMoving()) {
            Console.WriteLine("Elevator is already moving.");
            return;
        }
        if (IsOnFloor(floor)) {
            Console.WriteLine("Elevator is already on this floor.");
            return;
        }
        int direction = Math.Sign(floor - currentFloor);
        int distance = Math.Abs(floor - currentFloor);
        double maxSpeed = Math.Min(topSpeed, MaxSafeSpeed(distance)); // Calculate the maximum safe speed to reach the destination
        currentSpeed = 0;
        isMoving = true;
        destinationFloor = floor;
        Console.WriteLine("Elevator is moving to floor " + floor);
        while (!IsAtDestination()) {
            currentSpeed = Math.Min(currentSpeed + acceleration, (int)maxSpeed); // Increase speed up to the maximum safe speed
            double distanceToGo = DistanceToGo();
            double timeToStop = TimeToStop(distanceToGo);
            double timeToMaxSpeed = TimeToMaxSpeed(maxSpeed);
            double timeToTravel = TimeToTravel(distanceToGo, maxSpeed);
            double time = MinTimeToDestination(timeToStop, timeToMaxSpeed, timeToTravel);
            Thread.Sleep((int)(time * 1000)); // Wait for the time it takes to reach the destination
            position += direction * maxSpeed * time; // Update current position
            if (IsOnNewFloor(direction)) {
                UpdateCurrentFloor(direction);
                Console.WriteLine("Elevator is now at floor " + currentFloor);
            }
            if (IsAtDestination()) {
                Stop();
            }
        }
    }
    
    public void Stop() {
      double timeToStop = currentSpeed / brakingPower;
      double distanceToStop = (currentSpeed * currentSpeed) / (2 * brakingPower);
      double distanceToGo = Math.Abs(position - floorPositions[destinationFloor - 1]);

      if (distanceToStop < distanceToGo) {
        double timeToMaxSpeed = TimeToMaxSpeed(topSpeed);
        double timeToTravel = TimeToTravel(distanceToGo - distanceToStop, topSpeed);
        double minTime = MinTimeToDestination(timeToStop, timeToMaxSpeed, timeToTravel);

        currentSpeed = minTime < timeToMaxSpeed ? acceleration * minTime : topSpeed;
        position += currentSpeed * minTime + 0.5 * acceleration * minTime * minTime;
      } else {
        currentSpeed = Math.Sqrt(2 * brakingPower * distanceToGo);
        position = floorPositions[destinationFloor - 1];
      }

      UpdateCurrentFloor(direction * Math.Sign(position - floorPositions[currentFloor - 1]));
      direction = 0;
      destinationFloor = 0;
    }
    
    public double GetCurrentPosition() {
        return position;
    }
    
    private bool IsMoving() {
        return isMoving;
    }
    
    private bool IsOnFloor(int floor) {
        return currentFloor == floor;
    }
    
    private double MaxSafeSpeed(int distance) {
        return Math.Sqrt(2 * acceleration * (floorPositions[destinationFloor-1] - position)) / 2;
    }
    
    private double DistanceToGo() {
        return Math.Abs(floorPositions[destinationFloor-1] - position);
    }
    
    private double TimeToStop(double distanceToGo) {
        return Math.Sqrt(2 * distanceToGo / (double)
}
    private double TimeToMaxSpeed(double maxSpeed) {
        return (maxSpeed - currentSpeed) / acceleration;
    }
    
    private double TimeToTravel(double distanceToGo, double maxSpeed) {
        return distanceToGo / maxSpeed;
    }
    
    private double MinTimeToDestination(double timeToStop, double timeToMaxSpeed, double timeToTravel) {
        return Math.Min(timeToStop, Math.Min(timeToMaxSpeed, timeToTravel));
    }
    
    private bool IsAtDestination() {
        return Math.Abs(position - floorPositions[destinationFloor-1]) < closeEnough;
    }
    
    private bool IsOnNewFloor(int direction) {
        return Math.Sign(position - floorPositions[currentFloor-1]) != direction && IsAtFloor(position, destinationFloor);
    }
    
    private void UpdateCurrentFloor(int direction) {
        currentFloor += direction;
    }
    
    private bool IsAtFloor(double position, int floor) {
        return Math.Abs(position - floorPositions[floor-1]) < closeEnough;
    }
}
