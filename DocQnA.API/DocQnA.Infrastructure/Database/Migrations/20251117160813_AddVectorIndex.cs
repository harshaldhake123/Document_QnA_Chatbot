using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocQnA.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddVectorIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chunks_Documents_DocumentId",
                table: "Chunks");

            migrationBuilder.AlterColumn<Guid>(
                name: "DocumentId",
                table: "Chunks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chunks_Documents_DocumentId",
                table: "Chunks",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS idx_chunks_embedding
                    ON ""Chunks""
                    USING ivfflat (""Embedding"" vector_cosine_ops)
                    WITH (lists = 100);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chunks_Documents_DocumentId",
                table: "Chunks");

            migrationBuilder.AlterColumn<Guid>(
                name: "DocumentId",
                table: "Chunks",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Chunks_Documents_DocumentId",
                table: "Chunks",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id");

            migrationBuilder.Sql(@"DROP INDEX IF EXISTS idx_chunks_embedding;");
        }
    }
}