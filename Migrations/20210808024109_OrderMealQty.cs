using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodDelivery.Migrations
{
    public partial class OrderMealQty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealOrder");

            migrationBuilder.CreateTable(
                name: "Ordermeals",
                columns: table => new
                {
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    MealId = table.Column<long>(type: "bigint", nullable: false),
                    Qty = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordermeals", x => new { x.OrderId, x.MealId });
                    table.ForeignKey(
                        name: "FK_Ordermeals_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ordermeals_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ordermeals_MealId",
                table: "Ordermeals",
                column: "MealId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ordermeals");

            migrationBuilder.CreateTable(
                name: "MealOrder",
                columns: table => new
                {
                    MealsId = table.Column<long>(type: "bigint", nullable: false),
                    OrdersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealOrder", x => new { x.MealsId, x.OrdersId });
                    table.ForeignKey(
                        name: "FK_MealOrder_Meals_MealsId",
                        column: x => x.MealsId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealOrder_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealOrder_OrdersId",
                table: "MealOrder",
                column: "OrdersId");
        }
    }
}
